using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.Domain;

public class TestStatePredict
{
    private readonly IConfiguration _config;
    private readonly ILogger<LoggingDecoratorRetrieveWeather> _logger;

    public TestStatePredict()
    {
        _config = ConfigurationSingleton.Instance;

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<LoggingDecoratorRetrieveWeather>();
    }

    [Theory]
    [InlineData(2024, 5, 27, true)]
    [InlineData(2024, 5, 28, false)]
    [InlineData(2024, 5, 26, false)]
    [InlineData(2024, 12, 25, false)]
    public void TestIsMemorialDay(int year, int month, int day, bool assertResult)
    {
        // Arrange


        // Act


        // Assert

    }
}