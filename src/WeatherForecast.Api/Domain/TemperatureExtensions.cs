namespace WeatherForecast.Api.Domain;

internal static class TemperatureExtensions
{
    public static double ToFahrenheit(this double celsius) => 1.8 * celsius + 32;

    public static double ToKelvin(this double celsius) => celsius + 273.15;
}
