using System.Reflection;
using WeatherForecast.Api.TestFramework.Infrastructure;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework;

internal sealed class CustomTestFrameworkExecutor(
    AssemblyName assemblyName,
    ISourceInformationProvider sourceInformationProvider,
    IMessageSink diagnosticMessageSink)
    : XunitTestFrameworkExecutor(assemblyName, sourceInformationProvider, diagnosticMessageSink)
{
    private Dictionary<string, List<IXunitTestCase>> _testCasesByFeatureToggle;
    private Dictionary<string, List<string>> _featureTogglesByTestCaseId;
    private Dictionary<string, TestCollection> _testCollectionByFeatureToggles;

    // ReSharper disable once AsyncVoidMethod
    protected override async void RunTestCases(
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
    {
        var originalTestCases = testCases.ToArray();
        var newTestCases = SetupTestCases(originalTestCases);

        PrintDebugInfo(originalTestCases, newTestCases);

        using var assemblyRunner = new CustomTestAssemblyRunner(
            TestAssembly,
            newTestCases,
            DiagnosticMessageSink,
            executionMessageSink,
            executionOptions);

        await assemblyRunner.RunAsync();
    }

    private List<IXunitTestCase> SetupTestCases(IReadOnlyCollection<IXunitTestCase> testCases)
    {
        var result = new List<IXunitTestCase>(testCases.Count);

        InitializeDictionaries(testCases);

        foreach (var testCase in testCases)
        {
            var testCollectionId = Guid.NewGuid();
            var testCaseId = testCase.UniqueID;

            var isFeatured = _featureTogglesByTestCaseId[testCaseId].Count != 0 &&
                             ApiTestsSettings.UseFeaturedTestFramework;

            if (isFeatured)
            {
                foreach (var featureToggle in _featureTogglesByTestCaseId[testCaseId])
                {
                    if (HasFeatureToggleDefaultState(featureToggle))
                    {
                        result.Add(testCase);
                    }
                    else
                    {
                        result.Add(RecreateTestCaseWithTestCollection(
                            testCase: testCase,
                            testCollectionId: testCollectionId,
                            featureToggle: featureToggle));
                    }
                }
            }
            else
            {
                result.Add(testCase);
            }
        }

        return result;

        static bool HasFeatureToggleDefaultState(string featureToggle)
        {
            return string.Equals(featureToggle, FeatureToggles.Off, StringComparison.OrdinalIgnoreCase);
        }
    }

    private void InitializeDictionaries(IReadOnlyCollection<IXunitTestCase> testCases)
    {
        _testCasesByFeatureToggle = new Dictionary<string, List<IXunitTestCase>>(testCases.Count);
        _featureTogglesByTestCaseId = new Dictionary<string, List<string>>(testCases.Count);
        _testCollectionByFeatureToggles = new Dictionary<string, TestCollection>(testCases.Count);

        foreach (var testCase in testCases)
        {
            EnrichTestCasesAndFeatures(testCase);
        }
    }

    private void EnrichTestCasesAndFeatures(IXunitTestCase testCase)
    {
        var featureToggles = testCase
            .Method
            .GetCustomAttributes(typeof(FeatureToggleAttribute))
            .Select(attribute => attribute.GetNamedArgument<string>(nameof(FeatureToggleAttribute.Name)))
            .Distinct()
            .ToList();

        _featureTogglesByTestCaseId.TryAdd(testCase.UniqueID, featureToggles);

        foreach (var featureToggle in featureToggles)
        {
            if (_testCasesByFeatureToggle.TryGetValue(featureToggle, out var testCases))
            {
                testCases.Add(testCase);
            }
            else
            {
                _testCasesByFeatureToggle[featureToggle] = [testCase];
            }
        }
    }

    private XunitTestCase RecreateTestCaseWithTestCollection(
        IXunitTestCase testCase,
        Guid testCollectionId,
        string featureToggle = null)
    {
        var oldTestMethod = testCase.TestMethod;
        var oldTestClass = oldTestMethod.TestClass;
        var oldTestCollection = oldTestMethod.TestClass.TestCollection;

        TestCollection newTestCollection;
        if (featureToggle is null)
        {
            newTestCollection = new TestCollection(
                testAssembly: oldTestCollection.TestAssembly,
                collectionDefinition: oldTestCollection.CollectionDefinition,
                displayName: GetNewCollectionDisplayName(oldTestCollection, testCollectionId));
        }
        else
        {
            var key = $"{featureToggle}.{oldTestCollection.CollectionDefinition}";
            if (_testCollectionByFeatureToggles.TryGetValue(key, out var toggle))
            {
                newTestCollection = toggle;
            }
            else
            {
                newTestCollection = new TestCollection(
                    testAssembly: oldTestCollection.TestAssembly,
                    collectionDefinition: oldTestCollection.CollectionDefinition,
                    displayName: GetNewCollectionDisplayName(oldTestCollection, testCollectionId, featureToggle));
                _testCollectionByFeatureToggles.TryAdd(key, newTestCollection);
            }
        }

        var newTestClass = new TestClass(newTestCollection, oldTestClass.Class);
        var newTestMethod = new TestMethod(newTestClass, oldTestMethod.Method);
        var newTestCase = CreateTestCase(testCase, newTestMethod);

        return newTestCase;
    }

    private static string GetNewCollectionDisplayName(
        ITestCollection collection,
        Guid testCollectionId = default,
        string featureToggle = null)
    {
        var id = testCollectionId == default
            ? collection.UniqueID.ToString()
            : testCollectionId.ToString();

        return $"{collection.DisplayName}:{id}.toggle:{featureToggle ?? "null"}";
    }

    private XunitTestCase CreateTestCase(IXunitTestCase testCase, ITestMethod newTestMethod)
    {
        return testCase switch
        {
            XunitTheoryTestCase xunitTheoryTestCase => new XunitTheoryTestCase(
                diagnosticMessageSink: DiagnosticMessageSink,
                defaultMethodDisplay: GetTestMethodDisplay(xunitTheoryTestCase),
                defaultMethodDisplayOptions: GetTestMethodDisplayOptions(xunitTheoryTestCase),
                testMethod: newTestMethod),

            XunitTestCase xunitTestCase => new XunitTestCase(
                diagnosticMessageSink: DiagnosticMessageSink,
                defaultMethodDisplay: GetTestMethodDisplay(xunitTestCase),
                defaultMethodDisplayOptions: GetTestMethodDisplayOptions(xunitTestCase),
                testMethod: newTestMethod,
                testMethodArguments: xunitTestCase.TestMethodArguments),

            // Если Вы используете кастомные атрибуты (кроме Fact и Theory), то добавьте их обработку ниже

            _ => throw new ArgumentOutOfRangeException("Test case " + testCase.GetType() + " not supported")
        };
    }

    private static TestMethodDisplay GetTestMethodDisplay(TestMethodTestCase testCase)
    {
        return (TestMethodDisplay)typeof(TestMethodTestCase)
            .GetProperty("DefaultMethodDisplay", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(testCase)!;
    }

    private static TestMethodDisplayOptions GetTestMethodDisplayOptions(TestMethodTestCase testCase)
    {
        return (TestMethodDisplayOptions)typeof(TestMethodTestCase)
            .GetProperty("DefaultMethodDisplayOptions", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(testCase)!;
    }

    private static void PrintDebugInfo(
        IReadOnlyCollection<IXunitTestCase> originalTestCases,
        IReadOnlyCollection<IXunitTestCase> newTestCases)
    {
        var originalTestCollectionNames = new HashSet<string>(originalTestCases.Count);
        var newTestCollectionNames = new HashSet<string>(newTestCases.Count);

        foreach (var testCase in originalTestCases)
        {
            var testCollectionName = testCase.TestMethod.TestClass.TestCollection.DisplayName;
            originalTestCollectionNames.Add(testCollectionName);
            Console.WriteLine($"Original test case: '{testCase.DisplayName}' " +
                              $"in '{testCollectionName}' collection");
        }

        Console.WriteLine($"Original test cases count: {originalTestCases.Count}\n");

        foreach (var testCase in newTestCases)
        {
            var testCollectionName = testCase.TestMethod.TestClass.TestCollection.DisplayName;
            newTestCollectionNames.Add(testCollectionName);
            Console.WriteLine($"New test case: '{testCase.DisplayName}' " +
                              $"in '{testCollectionName}' collection");
        }

        Console.WriteLine($"New test cases count: {newTestCases.Count}\n");

        foreach (var testCollectionName in originalTestCollectionNames)
        {
            Console.WriteLine($"Original test collection name: '{testCollectionName}'");
        }

        Console.WriteLine($"Original test collections count: {originalTestCollectionNames.Count}\n");

        foreach (var testCollectionName in newTestCollectionNames)
        {
            Console.WriteLine($"New test collection name: '{testCollectionName}'");
        }

        Console.WriteLine($"New test collections count: {newTestCollectionNames.Count}\n");
    }
}
