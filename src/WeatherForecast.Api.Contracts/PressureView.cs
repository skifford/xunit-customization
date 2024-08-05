namespace WeatherForecast.Api.Contracts;

public sealed record PressureView
{
    public required double Pascal { get; init; }

    public required double MillimetersOfMercury { get; init; }

    public required double Bar { get; init; }
}
