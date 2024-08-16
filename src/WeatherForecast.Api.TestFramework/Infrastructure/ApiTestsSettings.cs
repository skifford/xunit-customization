namespace WeatherForecast.Api.TestFramework.Infrastructure;

public static class ApiTestsSettings
{
    private static Uri _gatewayHost;
    private static bool? _useFeaturedTestFramework;

    public static Uri GatewayHost
    {
        get
        {
            _gatewayHost ??= GetValue(EnvironmentVariables.Host, value =>
            {
                var success = Uri.TryCreate(value, UriKind.Absolute, out var result);
                return (success, result);
            });

            return _gatewayHost;
        }
    }

    public static bool UseFeaturedTestFramework
    {
        get
        {
            _useFeaturedTestFramework ??= GetValue(EnvironmentVariables.UseFeaturedTestFramework, value =>
            {
                var success = bool.TryParse(value, out var result);
                return (success, result);
            });

            return _useFeaturedTestFramework.Value;
        }
    }

    private static T GetValue<T>(
        string environmentVariableName,
        Func<string, (bool Succes, T Result)> func = null)
    {
        var value = Environment.GetEnvironmentVariable(
            variable: environmentVariableName,
            target: EnvironmentVariableTarget.Process);

        var result = func?.Invoke(value);

        if (result is null || result.Value.Succes is false)
        {
            throw new ApplicationException($"Required environment variable '{environmentVariableName}' must be set");
        }

        return result.Value.Result;
    }
}
