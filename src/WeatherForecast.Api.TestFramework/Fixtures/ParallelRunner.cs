using System.Collections.Concurrent;

namespace WeatherForecast.Api.TestFramework.Fixtures;

internal static class ParallelRunner
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> SemaphoreSlimsByTypes = new();

    public static async Task WaitAsync(string key, Func<Task> func)
    {
        var semaphoreSlim = SemaphoreSlimsByTypes.GetOrAdd(
            key: key,
            valueFactory: _ => new SemaphoreSlim(1));

        await semaphoreSlim.WaitAsync();

        try
        {
            await func.Invoke();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}
