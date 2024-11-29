using Microsoft.FeatureManagement;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Exceptions;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Domain;

internal sealed class WeatherForecastProvider(
    ITemperutureProvider temperutureProvider,
    IPressureProvider pressureProvider,
    IFeatureManager featureManager)
    : IWeatherForecastProvider
{
    private static readonly string[] Summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    public Task<SummaryView> GetSummary()
    {
        return Task.FromResult(new SummaryView
        {
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });
    }

    public async Task<DefaultWeatherForecastView> GetDefaultWeatherForecast()
    {
        if (await featureManager.IsEnabledAsync(Features.WeatherStation))
        {
            throw new EndpointObsoleteException("Endpoint is obsolete: use GET 'WeatherForecast/Advanced' instead");
        }

        return new DefaultWeatherForecastView
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(-20, 55)
        };
    }

    public async Task<AdvancedWeatherForecastView> GetAdvancedWeatherForecast()
    {
        var temperatureSensor = await temperutureProvider.GetSensor();
        var pressureSensor = await pressureProvider.GetSensor();

        var temperature = await temperatureSensor.GetCelsiusTemperature();
        var pressure = await pressureSensor.GetMillimetersOfMercuryPressure();

        return new AdvancedWeatherForecastView
        {
            Date = DateTimeOffset.Now,
            Temperature = new TemperatureView
            {
                Celsius = temperature,
                Fahrenheit = temperature.ToFahrenheit(),
                Kelvin = temperature.ToKelvin()
            },
            Pressure = new PressureView
            {
                MillimetersOfMercury = pressure,
                Pascal = pressure.ToPascal(),
                Bar = pressure.ToBar()
            }
        };
    }
}
