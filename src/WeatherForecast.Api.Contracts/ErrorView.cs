namespace WeatherForecast.Api.Contracts;

public sealed record ErrorView
{
    public required string Type { get; init; }

    public required string Message { get; init; }

    public required string[] StackTrace { get; init; }
}
