using WeatherForecast.Api.TestFramework.Fixtures;

namespace WeatherForecast.Api.Tests;

public sealed class WeatherForecastFixture : FixtureBase
{
    protected override async Task FixtureInitializeAsync()
    {
        await base.FixtureInitializeAsync();

        // Ваша логика инициализации разделяемого контекста
        // ...
    }

    protected override async Task FixtureDisposeAsync()
    {
        // Ваша логика освобождение разделяемого контекста
        // ...

        await base.FixtureDisposeAsync();
    }
}
