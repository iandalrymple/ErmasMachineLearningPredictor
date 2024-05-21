using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;
using Predictor.RetrieveOwmWeather.Implementations;

namespace Predictor.Testing.Domain;

public class TestStateWeather
{
    private readonly IConfiguration _config;
    private readonly ILogger<LoggingDecoratorRetrieveWeather> _logger;

    public TestStateWeather()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("testSettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<LoggingDecoratorRetrieveWeather>();
    }

    [Theory]
    [InlineData( 2024, 5, 15)]
    [InlineData(2024, 5, 22)]
    public async Task TestExecute_Happy(int year, int month, int day)
    {
        // Arrange
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var retrieverBaseObject = new RetrieveWeather(_config["BaseWeatherUri"]!, _config["AppId"]!);
        var decorator = new LoggingDecoratorRetrieveWeather(retrieverBaseObject, _logger);
        var sut = new StateWeather(decorator);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.Weather,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StateResultAggregatorModel(),
            DateToCheck = dateToCheck
        };

        // Act
        await sut.Execute(container);
        
        // Assert
        Assert.Equal( PredictorFsmStates.Weather + 1, container.CurrentState);
        Assert.NotNull(container.StateResults.StateWeatherResults);
        Assert.Equal(4, container.StateResults.StateWeatherResults.WeatherAtTimes.Count);
    }
}