namespace WeatherForecast.Api.Domain;

public interface IPressureProvider
{
    Task<IPressureSensor> GetSensor();
}
