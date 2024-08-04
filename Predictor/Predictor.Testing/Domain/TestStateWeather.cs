using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Supporting;
using OwmSqlite = Predictor.RetrieveOwmWeatherSqlite.Implementations;

namespace Predictor.Testing.Domain;

public class TestStateWeather
{
    private readonly IConfiguration _config;
    private readonly ILogger<RetrieveWeather> _logger;

    public TestStateWeather()
    {
        _config = ConfigurationSingleton.Instance;

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<RetrieveWeather>();
    }

    [Theory]
    [InlineData( 2024, 5, 15)]
    [InlineData(2024, 5, 22)]
    public async Task TestExecute_Happy(int year, int month, int day)
    {
        // Arrange
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var retrieverBaseObject = new RetrieveWeather(_config["BaseWeatherUri"]!, _config["AppId"]!, _logger, new OwmSqlite.RetrieveWeatherNullMock());
        var sut = new StateWeather(retrieverBaseObject);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.Weather,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StatesCombinedResultModel(),
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