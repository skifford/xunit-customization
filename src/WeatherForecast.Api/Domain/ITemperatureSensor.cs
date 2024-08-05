namespace WeatherForecast.Api.Domain;

public interface ITemperatureSensor
{
    Task<double> GetCelsiusTemperature();
}
