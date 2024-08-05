namespace WeatherForecast.Api.Contracts;

public sealed class TemperatureView
{
    public required double Celsius { get; init; }

    public required double Fahrenheit { get; init; }

    public required double Kelvin { get; init; }
}
