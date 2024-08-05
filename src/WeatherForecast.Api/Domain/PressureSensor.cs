namespace WeatherForecast.Api.Domain;

internal sealed class PressureSensor : IPressureSensor
{
    public Task<double> GetMillimetersOfMercuryPressure() => Task.FromResult((double)Random.Shared.Next(720, 800));
}
