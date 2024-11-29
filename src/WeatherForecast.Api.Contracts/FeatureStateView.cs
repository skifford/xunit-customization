namespace WeatherForecast.Api.Contracts;

public sealed record FeatureStateView
{
    public required string Feature { get; init; }

    public required bool? State { get; init; }
}
