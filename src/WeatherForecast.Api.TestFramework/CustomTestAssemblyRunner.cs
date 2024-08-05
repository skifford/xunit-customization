using Xunit.Abstractions;
using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework;

internal sealed class CustomTestAssemblyRunner(
    ITestAssembly testAssembly,
    IEnumerable<IXunitTestCase> testCases,
    IMessageSink diagnosticMessageSink,
    IMessageSink executionMessageSink,
    ITestFrameworkExecutionOptions executionOptions)
    : XunitTestAssemblyRunner(
        testAssembly,
        testCases,
        diagnosticMessageSink,
        executionMessageSink,
        executionOptions)
{
    protected override Task<RunSummary> RunTestCollectionAsync(
        IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource)
    {
        var testCollectionRunner = new CustomTestCollectionRunner(
            testCollection,
            testCases,
            DiagnosticMessageSink,
            messageBus,
            TestCaseOrderer,
            new ExceptionAggregator(Aggregator),
            cancellationTokenSource);

        return testCollectionRunner.RunAsync();
    }
}
