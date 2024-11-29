using Xunit;

namespace WeatherForecast.Api.Tests;

[CollectionDefinition(nameof(WeatherForecastFixture))]
public sealed class WeatherForecastCollection : ICollectionFixture<WeatherForecastFixture>;
