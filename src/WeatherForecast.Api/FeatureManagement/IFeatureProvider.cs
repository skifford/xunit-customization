namespace WeatherForecast.Api.FeatureManagement;

public interface IFeatureProvider
{
    Task SetFeatureState(string feature, bool state);

    Task<bool?> GetFeatureState(string feature);

    Task<IReadOnlyDictionary<string, bool>> GetAllFeatures();
}
