using Microsoft.FeatureManagement;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Domain;

internal sealed class TemperutureProvider(IFeatureManager featureManager) : ITemperutureProvider
{
    public async Task<ITemperatureSensor> GetSensor()
    {
        if (await featureManager.IsEnabledAsync(Features.WeatherStation))
        {
            return new WeatherStation();
        }

        return new TemperatureSensor();
    }
}
