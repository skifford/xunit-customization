namespace WeatherForecast.Api.Clients;

public sealed record WeatherForecastApiClientOptions
{
    public Uri BaseAddress { get; set; }
}
