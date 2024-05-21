using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    private readonly DateTime _dateToCheck;

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

        _dateToCheck = new DateTime(year: 2024, month: 5, day: 15);
    }

    [Fact]
    public async Task TestExecute_Happy()
    {
        // Arrange
        var retrieverBaseObject = new RetrieveWeather(_config["BaseWeatherUri"]!, _config["AppId"]!);
        var decorator = new LoggingDecoratorRetrieveWeather(retrieverBaseObject, _logger);
        var sut = new StateWeather(decorator);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.Weather,
            StoreLocation = _config.GetSection("StoreLocation").Get<List<StoreLocation>>()!.First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StateResultAggregatorModel(),
            DateToCheck = _dateToCheck
        };

        // Act
        await sut.Execute(container);
        
        // Assert
        Assert.Equal( PredictorFsmStates.Weather + 1, container.CurrentState);
        Assert.NotNull(container.StateResults.StateWeatherResults);
        Assert.Equal(4, container.StateResults.StateWeatherResults.WeatherAtTimes.Count);
    }
}