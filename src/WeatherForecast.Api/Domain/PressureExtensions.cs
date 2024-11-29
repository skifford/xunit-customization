namespace WeatherForecast.Api.Domain;

internal static class PressureExtensions
{
    public static double ToPascal(this double millimetersOfMercury) => millimetersOfMercury * 133.322;

    public static double ToBar(this double millimetersOfMercury) => millimetersOfMercury / 750.06168282;
}
