using System.Reflection;
using WeatherForecast.Api.TestFramework.Infrastructure;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WeatherForecast.Api.TestFramework;

internal sealed class CustomTestFramework : XunitTestFramework
{
    public CustomTestFramework(IMessageSink messageSink) : base(messageSink)
    {
        messageSink.OnMessage(new DiagnosticMessage($"Using {nameof(CustomTestFramework)}"));
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
    {
        return ApiTestsSettings.UseParallelTestFramework || ApiTestsSettings.UseFeaturedTestFramework
            ? new CustomTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink)
            : base.CreateExecutor(assemblyName);
    }
}
