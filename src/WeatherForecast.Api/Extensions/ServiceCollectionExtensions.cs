using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using WeatherForecast.Api.Domain;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services)
    {
        services.AddFeatureManagementServices();
        services.AddDomainServices();

        return services;
    }

    private static IServiceCollection AddFeatureManagementServices(this IServiceCollection services)
    {
        services.AddFeatureManagement();
        services.AddSingleton<IFeatureProvider, InMemoryFeatureProvider>();
        services.AddSingleton<IFeatureDefinitionProvider, FeatureDefinitionProvider>();
        services.AddSingleton<IDisabledFeaturesHandler, DisabledFeaturesHandler>();

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastProvider, WeatherForecastProvider>();
        services.AddScoped<ITemperutureProvider, TemperutureProvider>();
        services.AddScoped<IPressureProvider, PressureProvider>();

        return services;
    }
}
