namespace WeatherForecast.Api.TestFramework;

public static class Traits
{
    /// <summary>
    /// Категория тестов
    /// </summary>
    public const string Category = nameof(Category);

    public static class Categories
    {
        /// <summary>
        /// Используется запуска тестов, отмеченных как параллельные (с атрибутом [Parallel])
        /// </summary>
        public const string Parallel = nameof(Parallel);
    }
}
