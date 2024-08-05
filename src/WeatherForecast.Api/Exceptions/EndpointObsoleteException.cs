namespace WeatherForecast.Api.Exceptions;

public sealed class EndpointObsoleteException : Exception
{
    public EndpointObsoleteException()
    {
    }

    public EndpointObsoleteException(string message) : base(message)
    {
    }

    public EndpointObsoleteException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
