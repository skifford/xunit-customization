using WeatherForecast.Api.Clients;

namespace WeatherForecast.Api.TestFramework.Infrastructure;

public sealed class ApiFacade(IFeaturesClient features, IWeatherForecastClient weatherForecast)
{
    public IFeaturesClient Features { get; } = features;

    public IWeatherForecastClient WeatherForecast { get; } = weatherForecast;
}
