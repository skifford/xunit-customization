namespace WeatherForecast.Api.Domain;

public interface IPressureSensor
{
    Task<double> GetMillimetersOfMercuryPressure();
}
