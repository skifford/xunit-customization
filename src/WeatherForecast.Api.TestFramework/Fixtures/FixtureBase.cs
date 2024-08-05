using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Api.Clients.Extensions;
using WeatherForecast.Api.TestFramework.Infrastructure;
using Xunit;

namespace WeatherForecast.Api.TestFramework.Fixtures;

public abstract class FixtureBase : IAsyncLifetime, IFeatureToggleFixture
{
    public ApiFacade Api { get; }

    public IReadOnlyCollection<string> CurrentFeatureToggles { get; set; }

    protected FixtureBase()
    {
        Api = new ServiceCollection()
            .AddHttpClient()
            .AddWeatherForecastApiClients(options => options.BaseAddress = ApiTestsSettings.GatewayHost)
            .AddSingleton<CancellationTokenSource>()
            .AddScoped<ApiFacade>()
            .BuildServiceProvider()
            .GetRequiredService<ApiFacade>();
    }

    public Task SetFeatureToggleStates(bool state)
    {
        var tasks = CurrentFeatureToggles
            .Select(featureToggle => SetFeatureToggleState(featureToggle, state))
            .ToArray();

        return Task.WhenAll(tasks);
    }

    Task IAsyncLifetime.InitializeAsync()
    {
        // Ваша логика инициализации разделяемого контекста
        // ...

        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        // Ваша логика освобождения разделяемого контекста
        // ...

        return Task.CompletedTask;
    }

    private Task SetFeatureToggleState(string featureToggle, bool state)
    {
        return string.IsNullOrWhiteSpace(featureToggle)
            ? Task.CompletedTask
            : Api.Features.SetState(featureToggle, state);
    }
}
