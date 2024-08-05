namespace WeatherForecast.Api.TestFramework.Fixtures;

internal static class ParallelFixtureRunner
{
    /// <summary>
    /// В рамках одной Fixture (TestCollection) запускает делегат строго последовательно
    /// </summary>
    public static Task WaitAsync<TFixture>(TFixture fixture, Func<Task> func) where TFixture : IFixture
    {
        return ParallelRunner.WaitAsync(fixture.GetType().FullName!, func);
    }
}
