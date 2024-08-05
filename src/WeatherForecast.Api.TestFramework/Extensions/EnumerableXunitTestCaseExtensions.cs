using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework.Extensions;

internal static class EnumerableXunitTestCaseExtensions
{
    public static IReadOnlyCollection<string> GetFeatureToggles(this IEnumerable<IXunitTestCase> xunitTestCases)
    {
        var testCases = xunitTestCases.ToArray();
        var testCollectionDisplayNames = testCases.GetTestCollectionDisplayNames();

        return testCases
            .SelectMany(testCase => testCase.Method.GetCustomAttributes(typeof(FeatureToggleAttribute)))
            .Select(attribute => attribute.GetNamedArgument<string>(nameof(FeatureToggleAttribute.Name)))
            .Where(featureToggle => testCollectionDisplayNames
                .SelectMany(name => name.Split("toggle:"))
                .Contains(featureToggle))
            .ToHashSet();
    }

    private static HashSet<string> GetTestCollectionDisplayNames(this IEnumerable<IXunitTestCase> testCases)
    {
        return testCases
            .Select(testCase => testCase.TestMethod.TestClass.TestCollection.DisplayName)
            .ToHashSet();
    }
}
