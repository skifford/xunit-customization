using WeatherForecast.Api.TestFramework.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework;

internal sealed class CustomTestCollectionRunner(
    ITestCollection testCollection,
    IEnumerable<IXunitTestCase> testCases,
    IMessageSink diagnosticMessageSink,
    IMessageBus messageBus,
    ITestCaseOrderer testCaseOrderer,
    ExceptionAggregator aggregator,
    CancellationTokenSource cancellationTokenSource)
    : XunitTestCollectionRunner(
        testCollection,
        testCases,
        diagnosticMessageSink,
        messageBus,
        testCaseOrderer,
        aggregator,
        cancellationTokenSource)
{
    private IReadOnlyCollection<string> _featureToggles;

    protected override async Task AfterTestCollectionStartingAsync()
    {
        await base.AfterTestCollectionStartingAsync();

        if (TestCases.Any())
        {
            // Для классов с аттрибутом [Collection]
            _featureToggles = TestCases.GetFeatureToggles();
            if (_featureToggles.Count == 0)
            {
                return;
            }

            foreach (var fixture in CollectionFixtureMappings.Values)
            {
                fixture.SetFeatureToggles(_featureToggles);
                await fixture.EnableFeatureToggles();

                Console.WriteLine(
                    $"For fixture '{fixture}' " +
                    $"toggles '{GetToggleNames()}' activated " +
                    $"in '{GetCollectionName()}' collection");
            }
        }
    }

    protected override async Task BeforeTestCollectionFinishedAsync()
    {
        if (_featureToggles.Count != 0)
        {
            foreach (var fixture in CollectionFixtureMappings.Values)
            {
                await fixture.DisableFeatureToggles();

                Console.WriteLine(
                    $"For fixture '{fixture}' " +
                    $"all toggles deactivated in '{GetCollectionName()}' collection\n");
            }
        }

        await base.BeforeTestCollectionFinishedAsync();
    }

    protected override Task<RunSummary> RunTestClassAsync(
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases)
    {
        var testClassRunner = new CustomTestClassRunner(
            testClass,
            @class,
            testCases,
            DiagnosticMessageSink,
            MessageBus,
            TestCaseOrderer,
            Aggregator,
            CancellationTokenSource,
            CollectionFixtureMappings);

        return testClassRunner.RunAsync();
    }

    private string GetCollectionName() => TestCollection.DisplayName;

    private string GetToggleNames() => string.Join(", ", _featureToggles);
}
