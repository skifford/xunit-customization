using Refit;
using WeatherForecast.Api.Contracts;

namespace WeatherForecast.Api.Clients;

public interface IFeaturesClient
{
    [Get("/Features")]
    Task<IReadOnlyCollection<FeatureStateView>> GetFeatures();

    [Get("/Features/{feature}/State")]
    Task<FeatureStateView> GetFeature(
        [Query] string feature,
        CancellationToken cancellationToken = default);

    [Put("/Features/{feature}/State")]
    Task<FeatureStateView> SetState(
        [Query] string feature,
        [Query] bool state,
        CancellationToken cancellationToken = default);
}
