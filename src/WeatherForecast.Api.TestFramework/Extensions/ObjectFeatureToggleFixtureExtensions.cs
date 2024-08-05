using WeatherForecast.Api.TestFramework.Fixtures;

namespace WeatherForecast.Api.TestFramework.Extensions;

internal static class ObjectFeatureToggleFixtureExtensions
{
    public static void SetFeatureToggles(this object instance, IReadOnlyCollection<string> featureToggles)
    {
        var type = instance.GetType();

        type
            .GetProperty(nameof(IFeatureToggleFixture.CurrentFeatureToggles))?
            .GetSetMethod()?
            .Invoke(instance, [featureToggles]);
    }

    public static Task EnableFeatureToggles(this object instance)
    {
        return instance.SetFeatureTogglesState(enabled: true);
    }

    public static Task DisableFeatureToggles(this object instance)
    {
        return instance.SetFeatureTogglesState(enabled: false);
    }

    private static Task SetFeatureTogglesState(this object instance, bool enabled)
    {
        var type = instance.GetType();

        var task = type
            .GetMethod(nameof(IFeatureToggleFixture.SetFeatureToggleStates))?
            .Invoke(instance, [enabled]) as Task;

        return task ?? Task.CompletedTask;
    }
}
