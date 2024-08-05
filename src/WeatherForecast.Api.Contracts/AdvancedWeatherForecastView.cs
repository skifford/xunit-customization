namespace WeatherForecast.Api.Contracts;

public sealed record AdvancedWeatherForecastView
{
    public required DateTimeOffset Date { get; init; }

    public required TemperatureView Temperature { get; init; }

    public required PressureView Pressure { get; init; }
}
