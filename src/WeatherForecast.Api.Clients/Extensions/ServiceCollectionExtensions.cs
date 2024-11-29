using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace WeatherForecast.Api.Clients.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForecastApiClients(
        this IServiceCollection services,
        Action<WeatherForecastApiClientOptions> configure)
    {
        services.Configure(configure);
        services.AddClient<IWeatherForecastClient>();
        services.AddClient<IFeaturesClient>();

        return services;
    }

    private static void AddClient<TClient>(this IServiceCollection services) where TClient : class
    {
        services
            .AddRefitClient<TClient>(SettingsAction)
            .ConfigureHttpClient(ConfigureClient);
    }

    private static RefitSettings SettingsAction(IServiceProvider provider)
    {
        return new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                })
        };
    }

    private static void ConfigureClient(IServiceProvider provider, HttpClient client)
    {
        var options = provider.GetRequiredService<IOptions<WeatherForecastApiClientOptions>>().Value;
        client.BaseAddress = options.BaseAddress;
    }
}
