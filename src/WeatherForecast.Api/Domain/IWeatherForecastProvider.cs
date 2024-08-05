using WeatherForecast.Api.Contracts;

namespace WeatherForecast.Api.Domain;

public interface IWeatherForecastProvider
{
    Task<SummaryView> GetSummary();

    Task<DefaultWeatherForecastView> GetDefaultWeatherForecast();

    Task<AdvancedWeatherForecastView> GetAdvancedWeatherForecast();
}
