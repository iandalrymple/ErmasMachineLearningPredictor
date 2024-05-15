using Microsoft.Extensions.Configuration;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Mocks;
using RestSharp;

namespace Predictor.Testing.RetrieveOwmWeather;

public class TestLoggingDecoratedRetrieveWeather
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("testSettings.json")
        .Build();
    public DateTime PastDateTime =
        DateTime.SpecifyKind(new DateTime(year: 2023, month: 4, day: 19), DateTimeKind.Utc);
    public DateTime FutureDateTime = 
        DateTime.SpecifyKind(DateTime.UtcNow.AddDays(3), DateTimeKind.Utc);

    [Fact]
    public async Task TestRetrieveDecorated()
    {
        // Create the options for RestClient - method above checked null.
        var rco = new RestClientOptions(_configuration["BaseWeatherUri"]!);

        // Create the OUT
        var obj = new RetrieveWeather(new RestClient(rco), new RestRequest());

        // Mock logger
        var loggerMock = new MockLogger();

        // Now create the decorator. 
        var decorator = new LoggingDecoratorRetrieveWeather(obj, loggerMock);

        // Spin up the params object 
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            AppId = _configuration["AppId"]!,
            DateTime = PastDateTime
        };

        // Now get the results for some special day.
        var result = await decorator.Retrieve(paramObject);

        // Test the result 
        Assert.True(CheckWeather(result, PastDateTime));
    }

    [Fact]
    public async Task TestRetrieveDecoratedFuture()
    {
        // Create the options for RestClient - method above checked null.
        var rco = new RestClientOptions(_configuration["BaseWeatherUri"]!);

        // Create the OUT
        var obj = new RetrieveWeather(new RestClient(rco), new RestRequest());

        // Mock logger
        var loggerMock = new MockLogger();

        // Now create the decorator. 
        var decorator = new LoggingDecoratorRetrieveWeather(obj, loggerMock);

        // Spin up the params object 
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            AppId = _configuration["AppId"]!,
            DateTime = FutureDateTime
        };

        // Now get the results for some special day.
        var result = await decorator.Retrieve(paramObject);

        // Test the result 
        Assert.True(CheckWeather(result, FutureDateTime));
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