using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Api.Clients.Extensions;
using WeatherForecast.Api.TestFramework.Infrastructure;
using Xunit;

namespace WeatherForecast.Api.TestFramework.Fixtures;

public abstract class FixtureBase : IAsyncLifetime, IFixture, IFeatureToggleFixture
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

    /// <summary>
    /// Не переопределять и не реализовывать в производных классах. <br />
    /// Необходимо использовать <seealso cref="FixtureInitializeAsync"/>
    /// </summary>
    Task IAsyncLifetime.InitializeAsync()
    {
        return SingleExecuteAsync(async () => await FixtureInitializeAsync(), (IFixture)this);
    }

    /// <summary>
    /// Не переопределять и не реализовывать в производных классах. <br />
    /// Необходимо использовать <seealso cref="FixtureDisposeAsync"/>
    /// </summary>
    async Task IAsyncLifetime.DisposeAsync()
    {
        await SingleExecuteAsync(async () => await FixtureDisposeAsync(), (IFixture)this);
    }

    protected virtual Task FixtureInitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task FixtureDisposeAsync()
    {
        return Task.CompletedTask;
    }

    private Task SetFeatureToggleState(string featureToggle, bool state)
    {
        return string.IsNullOrWhiteSpace(featureToggle)
            ? Task.CompletedTask
            : Api.Features.SetState(featureToggle, state);
    }

    private static async Task SingleExecuteAsync<TFixture>(Func<Task> func, TFixture fixture) where TFixture : IFixture
    {
        if (ApiTestsSettings.UseParallelTestFramework)
        {
            await ParallelFixtureRunner.WaitAsync(fixture, async () =>
            {
                if (FixturesManager.ContainsFixture(fixture))
                {
                    return;
                }

                await func.Invoke();

                FixturesManager.AddFixture(fixture);
            });
        }
        else
        {
            await func.Invoke();
        }
    }
}
