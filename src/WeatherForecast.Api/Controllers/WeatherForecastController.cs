using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Domain;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class WeatherForecastController(IWeatherForecastProvider weatherForecastProvider) : ControllerBase
{
    /// <summary>
    /// Состояние фичи 'WeatherStation' НЕ ВЛИЯЕТ на работу роута: в ответе будет краткое сообщение о погоде.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Summary")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SummaryView))]
    public Task<SummaryView> GetSummary()
    {
        return weatherForecastProvider.GetSummary();
    }

    /// <summary>
    /// Роут для работы только с НЕ активированой фичей 'WeatherStation':
    /// <br/> Если фича 'WeatherStation' НЕ активирована, то в ответе будет прогноз погоды по умолчанию.
    /// <br/> Если фича 'WeatherStation' АКТИВИРОВАНА, то будет ошибка 405.
    /// </summary>
    [HttpGet("Default")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DefaultWeatherForecastView))]
    [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed, Type = typeof(ErrorView))]
    public Task<DefaultWeatherForecastView> DefaultToday()
    {
        return weatherForecastProvider.GetDefaultWeatherForecast();
    }

    /// <summary>
    /// Роут для работы только с АКТИВИРОВАННОЙ фичей 'WeatherStation':
    /// <br/> Если фича 'WeatherStation' АКТИВИРОВАНА, то в ответе будет подробный прогноз погоды с метеостанции.
    /// <br/> Если фича 'WeatherStation' НЕ активирована, то будет ошибка 405.
    /// </summary>
    [FeatureGate(Features.WeatherStation)]
    [HttpGet("Advanced")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AdvancedWeatherForecastView))]
    [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed, Type = typeof(ErrorView))]
    public Task<AdvancedWeatherForecastView> AdvancedToday()
    {
        return weatherForecastProvider.GetAdvancedWeatherForecast();
    }
}
