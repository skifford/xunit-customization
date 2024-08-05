using WeatherForecast.Api.TestFramework;
using WeatherForecast.Api.TestFramework.Fixtures;
using WeatherForecast.Api.TestFramework.Infrastructure;
using Xunit;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace WeatherForecast.Api.Tests;

[Trait(Traits.Category, Traits.Categories.Parallel)]
[Parallel]
[Collection(nameof(WeatherForecastFixture))]
public sealed class WeatherForecastTests
{
    private readonly ApiFacade _api;

    public WeatherForecastTests(WeatherForecastFixture fixture)
    {
        FixturesManager.SetActualFixture(ref fixture);
        _api = fixture.Api;
    }

    /// <summary>
    /// Тест будет запускаться дважды: без фичей и с активированной фичей 'WeatherStation'
    /// </summary>
    [Fact]
    [FeatureToggle(FeatureToggles.Off)]
    [FeatureToggle(FeatureToggles.WeatherStation)]
    public async Task GetSummary_ValidModel_Ok()
    {
        // Arrange

        // Act
        var result = await _api.WeatherForecast.GetSummary();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Summary);
    }

    /// <summary>
    /// Тест будет запускаться без фичей
    /// </summary>
    [Fact]
    [FeatureToggle(FeatureToggles.Off)]
    public async Task DefaultToday_ValidModel_Ok()
    {
        // Arrange

        // Act
        var result = await _api.WeatherForecast.DefaultToday();

        // Assert
        Assert.NotNull(result);
    }

    /// <summary>
    /// Тест будет запускаться только с активированной фичей 'WeatherStation'
    /// </summary>
    [Fact]
    [FeatureToggle(FeatureToggles.WeatherStation)]
    public async Task AdvancedToday_ValidModel_Ok()
    {
        // Arrange

        // Act
        var result = await _api.WeatherForecast.AdvancedToday();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Date == default);
        Assert.NotNull(result.Temperature);
        Assert.NotNull(result.Pressure);
    }
}
