using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Supporting;
using OwmSqlite = Predictor.RetrieveOwmWeatherSqlite.Implementations;

namespace Predictor.Testing.RetrieveOwmWeather;

public class TestLoggingDecoratedRetrieveWeather
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RetrieveWeather> _logger;
    private readonly DateTime _pastDateTime;
    private readonly DateTime _futureDateTime;

    public TestLoggingDecoratedRetrieveWeather()
    {
        _configuration = ConfigurationSingleton.Instance;

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<RetrieveWeather>();

        _pastDateTime = DateTime.SpecifyKind(new DateTime(year: 2023, month: 4, day: 19), DateTimeKind.Utc);
        _futureDateTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(3), DateTimeKind.Utc);
    }

    [Fact]
    public async Task TestRetrieveDecorated()
    {
        // Arrange
        var retriever = new RetrieveWeather(_configuration["BaseWeatherUri"]!, _configuration["AppId"]!, _logger, new OwmSqlite.RetrieveWeatherNullMock());
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            DateTime = _pastDateTime
        };

        // Act
        var result = await retriever.Retrieve(paramObject);

        // Assert
        Assert.NotNull(result);
        Assert.True(CheckWeather(result, _pastDateTime));
    }

    [Fact]
    public async Task TestRetrieveDecoratedFuture()
    {
        // Arrange
        var retriever = new RetrieveWeather(_configuration["BaseWeatherUri"]!, _configuration["AppId"]!, _logger, new OwmSqlite.RetrieveWeatherNullMock());
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            DateTime = _futureDateTime
        };

        // Act
        var result = await retriever.Retrieve(paramObject);

        // Assert
        Assert.NotNull(result);
        Assert.True(CheckWeather(result, _futureDateTime));
    }

    internal static bool CheckWeather(WeatherSourceModel inModel, DateTime inDateTime)
    {
        // Check for null 
        ArgumentNullException.ThrowIfNull(inModel);
        if (inModel.Data == null) throw new ArgumentNullException(nameof(inModel));

        // Just verify the unix time
        return inModel.Data[0].Dt == ((DateTimeOffset)inDateTime).ToUnixTimeSeconds();
    }
}