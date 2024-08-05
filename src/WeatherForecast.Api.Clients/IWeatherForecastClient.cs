using Refit;
using WeatherForecast.Api.Contracts;

namespace WeatherForecast.Api.Clients;

public interface IWeatherForecastClient
{
    [Get("/WeatherForecast/Summary")]
    Task<SummaryView> GetSummary(CancellationToken cancellationToken = default);

    [Get("/WeatherForecast/Default")]
    Task<DefaultWeatherForecastView> DefaultToday(CancellationToken cancellationToken = default);

    [Get("/WeatherForecast/Advanced")]
    Task<AdvancedWeatherForecastView> AdvancedToday(CancellationToken cancellationToken = default);
}
