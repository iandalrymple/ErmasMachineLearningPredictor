using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;

namespace Predictor.Testing.RetrieveOwmWeather;

public class TestLoggingDecoratedRetrieveWeather
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoggingDecoratorRetrieveWeather> _logger;
    private readonly DateTime _pastDateTime;
    private readonly DateTime _futureDateTime;

    public TestLoggingDecoratedRetrieveWeather()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("testSettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<LoggingDecoratorRetrieveWeather>();

        _pastDateTime = DateTime.SpecifyKind(new DateTime(year: 2023, month: 4, day: 19), DateTimeKind.Utc);
        _futureDateTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(3), DateTimeKind.Utc);
    }

    [Fact]
    public async Task TestRetrieveDecorated()
    {
        // Arrange
        var obj = new RetrieveWeather(_configuration["BaseWeatherUri"]!);
        var decorator = new LoggingDecoratorRetrieveWeather(obj, _logger);
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            AppId = _configuration["AppId"]!,
            DateTime = _pastDateTime
        };

        // Act
        var result = await decorator.Retrieve(paramObject);

        // Assert
        Assert.True(CheckWeather(result, _pastDateTime));
    }

    [Fact]
    public async Task TestRetrieveDecoratedFuture()
    {
        // Arrange
        var obj = new RetrieveWeather(_configuration["BaseWeatherUri"]!);
        var decorator = new LoggingDecoratorRetrieveWeather(obj, _logger);
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            AppId = _configuration["AppId"]!,
            DateTime = _futureDateTime
        };

        // Act
        var result = await decorator.Retrieve(paramObject);

        // Assert
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