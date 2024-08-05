using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.FeatureManagement;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class FeaturesController(IFeatureProvider featureProvider) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IReadOnlyCollection<FeatureStateView>))]
    public async Task<ActionResult<IReadOnlyCollection<FeatureStateView>>> GetFeatures()
    {
        var features = await featureProvider.GetAllFeatures();
        var result = features
            .Select(kvp => new FeatureStateView
            {
                Feature = kvp.Key,
                State = kvp.Value
            })
            .ToArray();

        return Ok(result);
    }

    [HttpGet("{feature}/State")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FeatureStateView))]
    public async Task<ActionResult<FeatureStateView>> GetFeature([FromRoute][Required] string feature)
    {
        var result = new FeatureStateView
        {
            Feature = feature,
            State = await featureProvider.GetFeatureState(feature)
        };

        return Ok(result);
    }

    [HttpPut("{feature}/State")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FeatureStateView))]
    public async Task<ActionResult<FeatureStateView>> SetState(
        [FromRoute][Required] string feature,
        [FromQuery][Required] bool state)
    {
        await featureProvider.SetFeatureState(feature, state);

        return new FeatureStateView
        {
            Feature = feature,
            State = await featureProvider.GetFeatureState(feature)
        };
    }
}
