namespace WeatherForecast.Api.Exceptions;

public sealed class EndpointNotAllowedException : Exception
{
    public EndpointNotAllowedException()
    {
    }

    public EndpointNotAllowedException(string message) : base(message)
    {
    }

    public EndpointNotAllowedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
