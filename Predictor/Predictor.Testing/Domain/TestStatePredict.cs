using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;
using Predictor.Domain.System;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Mocks;
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

    [Fact]
    public async Task TestPrintCsv()
    {
        // Arrange
        var rawWeatherString = Properties.Resources.WeatherData_05152024;
        var rawWeatherModel = JsonConvert.DeserializeObject<StateWeatherResultModel>(rawWeatherString);
        var dateToCheck = new DateTime(year: 2024, month: 5, day: 15);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.Aggregate,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StatesCombinedResultModel
            {
                StateWeatherResults = rawWeatherModel,
                StateHistoricSalesResults = new StateHistoricSalesResultModel
                {
                    SalesDayBefore = 2535.0m,
                    SalesTwoDaysBefore = 4500.3m
                },
                StateCurrentSalesResults = new StateCurrentSalesResultModel
                {
                    SalesAtThree = 2500.0m,
                    FirstOrderMinutesInDay = 680,
                    LastOrderMinutesInDay = 1350
                }
            },
            DateToCheck = dateToCheck
        };
        var sut = new StateAggregate(new RetrieveHolidaysMock());
        await sut.Execute(container);

        // Act 
        var csv = container.StateResults.StateAggregateResults!.CreateCsvRows();

        // Assert
        Assert.NotNull(csv);
    }
}