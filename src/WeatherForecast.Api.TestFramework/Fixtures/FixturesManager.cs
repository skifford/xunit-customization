using WeatherForecast.Api.TestFramework.Infrastructure;

namespace WeatherForecast.Api.TestFramework.Fixtures;

public static class FixturesManager
{
    private static readonly Dictionary<Type, IFixture> FixturesByTypes = new();

    public static bool ContainsFixture<TFixture>(TFixture fixture)
    {
        return FixturesByTypes.ContainsKey(fixture.GetType());
    }

    public static void AddFixture<TFixture>(TFixture fixture) where TFixture : IFixture
    {
        FixturesByTypes.TryAdd(fixture.GetType(), fixture);
    }

    /// <summary>
    /// Подменяет входную Fixture на актуальную в случае параллельного запуска API тестов. <br />
    /// В текущей реализации этот метод должен вызываться всегда первым в конструкторе тестового класса.<br />
    /// Если его забыть вызвать при параллельном запуске API тестов, все кроме одного будут отваливаться.
    /// </summary>
    public static void SetActualFixture<TFixture>(ref TFixture fixture) where TFixture : IFixture
    {
        if (ApiTestsSettings.UseParallelTestFramework is false || fixture is null)
        {
            return;
        }

        var key = fixture.GetType();

        if (FixturesByTypes.TryGetValue(key, out var value))
        {
            fixture = (TFixture)value;
        }
    }
}
