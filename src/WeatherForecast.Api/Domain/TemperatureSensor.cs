namespace WeatherForecast.Api.Domain;

internal sealed class TemperatureSensor : ITemperatureSensor
{
    public Task<double> GetCelsiusTemperature() => Task.FromResult((double)Random.Shared.Next(-40, 40));
}
