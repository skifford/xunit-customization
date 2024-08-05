namespace WeatherForecast.Api.Contracts;

public sealed record DefaultWeatherForecastView
{
    public required DateOnly Date { get; init; }

    public required double TemperatureC { get; init; }
}
