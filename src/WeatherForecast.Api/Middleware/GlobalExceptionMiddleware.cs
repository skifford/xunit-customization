using System.Net;
using System.Text.Json;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Exceptions;

namespace WeatherForecast.Api.Middleware;

internal sealed class GlobalExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var statusCode = exception switch
            {
                EndpointNotAllowedException => HttpStatusCode.MethodNotAllowed,
                EndpointObsoleteException => HttpStatusCode.MethodNotAllowed,
                _ => HttpStatusCode.InternalServerError
            };

            var response = context.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new ErrorView
            {
                Type = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = exception.StackTrace?.Split("\r\n") ?? []
            });

            await response.WriteAsync(result);
        }
    }
}
