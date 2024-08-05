using Microsoft.FeatureManagement;

namespace WeatherForecast.Api.FeatureManagement;

internal sealed class FeatureDefinitionProvider(IFeatureProvider featureProvider) : IFeatureDefinitionProvider
{
    public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
    {
        var featureState = await featureProvider.GetFeatureState(featureName);

        return featureState.HasValue
            ? GetFeatureDefinition(featureName, featureState.Value)
            : null;
    }

    public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
    {
        var features = await featureProvider.GetAllFeatures();

        foreach (var (featureName, featureState) in features)
        {
            yield return GetFeatureDefinition(featureName, featureState);
        }
    }

    private static FeatureDefinition GetFeatureDefinition(string featureName, bool isEnabled)
    {
        return isEnabled
            ? CreateEnabledFeatureDefinition(featureName)
            : CreateDisabledFeatureDefinition(featureName);
    }

    private static FeatureDefinition CreateEnabledFeatureDefinition(string featureName)
    {
        return new FeatureDefinition
        {
            Name = featureName,
            EnabledFor = new[]
            {
                new FeatureFilterConfiguration
                {
                    Name = "AlwaysOn"
                }
            }
        };
    }

    private static FeatureDefinition CreateDisabledFeatureDefinition(string featureName)
    {
        return new FeatureDefinition
        {
            Name = featureName
        };
    }
}
