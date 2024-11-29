using System.Collections.Concurrent;

namespace WeatherForecast.Api.FeatureManagement;

internal sealed class InMemoryFeatureProvider : IFeatureProvider
{
    private static readonly ConcurrentDictionary<string, bool> Features = new();

    public Task SetFeatureState(string feature, bool state)
    {
        if (Features.ContainsKey(feature))
        {
            Features.TryGetValue(feature, out var comparison);
            Features.TryUpdate(feature, state, comparison);
        }
        else
        {
            Features.TryAdd(feature, state);
        }

        return Task.CompletedTask;
    }

    public Task<bool?> GetFeatureState(string feature)
    {
        return Features.TryGetValue(feature, out var state)
            ? Task.FromResult((bool?)state)
            : Task.FromResult((bool?)null);
    }

    public Task<IReadOnlyDictionary<string, bool>> GetAllFeatures()
    {
        return Task.FromResult((IReadOnlyDictionary<string, bool>)Features);
    }
}
