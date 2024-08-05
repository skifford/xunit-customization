namespace WeatherForecast.Api.Domain;

internal sealed class WeatherStation : ITemperatureSensor, IPressureSensor
{
    public Task<double> GetCelsiusTemperature() => Task.FromResult((double)Random.Shared.Next(-90, 90));

    public Task<double> GetMillimetersOfMercuryPressure() => Task.FromResult((double)Random.Shared.Next(660, 860));
}
