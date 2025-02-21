using WeatherForecast.Api.TestFramework.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework;

internal sealed class CustomTestClassRunner(
    ITestClass testClass,
    IReflectionTypeInfo @class,
    IEnumerable<IXunitTestCase> testCases,
    IMessageSink diagnosticMessageSink,
    IMessageBus messageBus,
    ITestCaseOrderer testCaseOrderer,
    ExceptionAggregator aggregator,
    CancellationTokenSource cancellationTokenSource,
    IDictionary<Type, object> collectionFixtureMappings)
    : XunitTestClassRunner(
        testClass,
        @class,
        testCases,
        diagnosticMessageSink,
        messageBus,
        testCaseOrderer,
        aggregator,
        cancellationTokenSource,
        collectionFixtureMappings)
{
    private IReadOnlyCollection<string> _featureToggles;

    protected override async Task AfterTestClassStartingAsync()
    {
        await base.AfterTestClassStartingAsync();

        if (TestCases.Any())
        {
            // Для классов, реализующих IClassFixture
            _featureToggles = TestCases.GetFeatureToggles();
            if (_featureToggles.Count == 0)
            {
                return;
            }

            foreach (var fixture in ClassFixtureMappings.Values)
            {
                fixture.SetFeatureToggles(_featureToggles);
                await fixture.EnableFeatureToggles();

                Console.WriteLine(
                    $"For fixture '{fixture}' " +
                    $"toggles '{GetToggleNames()}' activated " +
                    $"in '{TestClass.TestCollection.CollectionDefinition.Name}' collection");
            }
        }
    }

    protected override async Task BeforeTestClassFinishedAsync()
    {
        if (_featureToggles.Count != 0)
        {
            foreach (var fixture in ClassFixtureMappings.Values)
            {
                await fixture.DisableFeatureToggles();

                Console.WriteLine(
                    $"For fixture '{fixture}' " +
                    $"all toggles deactivated in '{TestClass.TestCollection.CollectionDefinition.Name}' collection\n");
            }
        }

        await base.BeforeTestClassFinishedAsync();
    }

    private string GetToggleNames() => string.Join(", ", _featureToggles);
}
