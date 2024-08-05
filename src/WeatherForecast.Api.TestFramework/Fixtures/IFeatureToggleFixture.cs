namespace WeatherForecast.Api.TestFramework.Fixtures;

internal interface IFeatureToggleFixture
{
    IReadOnlyCollection<string> CurrentFeatureToggles { get; set; }

    Task SetFeatureToggleStates(bool state);
}
