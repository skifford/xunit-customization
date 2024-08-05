namespace WeatherForecast.Api.Domain;

public interface ITemperutureProvider
{
    Task<ITemperatureSensor> GetSensor();
}
