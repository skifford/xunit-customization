namespace WeatherForecast.Api.Contracts;

public sealed record SummaryView
{
    public required string Summary { get; init; }
}
