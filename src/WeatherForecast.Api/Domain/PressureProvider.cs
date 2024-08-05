using Microsoft.FeatureManagement;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Domain;

internal sealed class PressureProvider(IFeatureManager featureManager) : IPressureProvider
{
    public async Task<IPressureSensor> GetSensor()
    {
        if (await featureManager.IsEnabledAsync(Features.WeatherStation))
        {
            return new WeatherStation();
        }

        return new PressureSensor();
    }
}
