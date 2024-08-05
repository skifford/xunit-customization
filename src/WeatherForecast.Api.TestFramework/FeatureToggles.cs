namespace WeatherForecast.Api.TestFramework;

public static class FeatureToggles
{
    /// <summary>
    /// Используется для коллекций тестов по умолчанию, без фичей
    /// </summary>
    public const string Off = nameof(Off);

    /// <summary>
    /// Используется для активации/деактивации фичи 'WeatherStation'
    /// </summary>
    public const string WeatherStation = nameof(WeatherStation);
}
