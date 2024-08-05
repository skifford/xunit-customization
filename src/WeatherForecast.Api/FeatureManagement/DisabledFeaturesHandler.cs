using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;
using WeatherForecast.Api.Exceptions;

namespace WeatherForecast.Api.FeatureManagement;

internal sealed class DisabledFeaturesHandler : IDisabledFeaturesHandler
{
    public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
    {
        var endpoint = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.GetDisplayUrl()}";
        throw new EndpointNotAllowedException(
            $"Endpoint '{endpoint}' not allowed for disabled features: '{string.Join(", ", features)}'");
    }
}
