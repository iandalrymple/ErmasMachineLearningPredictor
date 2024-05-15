using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;
using RestSharp;

namespace Predictor.Testing.RetrieveOwmWeather;

public class TestLoggingDecoratedRetrieveWeather
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("testSettings.json")
        .Build();
    public DateTime TestDateTime =
        DateTime.SpecifyKind(new DateTime(year: 2023, month: 4, day: 19), DateTimeKind.Utc);

    [Fact]
    public async Task TestRetrieveDecorated()
    {
        // Create the options for RestClient - method above checked null.
        var rco = new RestClientOptions(_configuration["BaseWeatherUri"]!);

        // Create the OUT
        var obj = new RetrieveWeather(new RestClient(rco), new RestRequest());

        // Mock logger
        var loggerMock = new Mock<ILogger<RetrieveWeather>>();

        // Now create the decorator. 
        var decorator = new LoggingDecoratorRetrieveWeather(obj, loggerMock.Object);

        // Spin up the params object 
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = Convert.ToDouble(_configuration["Lat_1"]),
            Longitude = Convert.ToDouble(_configuration["Lon_1"]),
            AppId = _configuration["AppId"]!,
            DateTime = TestDateTime
        };

        // Now get the results for some special day.
        var result = await decorator.Retrieve(paramObject);

        // Test the result 
        Assert.True(CheckWeather(result, TestDateTime));
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