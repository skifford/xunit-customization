namespace WeatherForecast.Api.TestFramework;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class FeatureToggleAttribute(string feature) : Attribute
{
    public string Name { get; } = string.IsNullOrWhiteSpace(feature)
        ? throw new ArgumentNullException(feature)
        : feature;
}
