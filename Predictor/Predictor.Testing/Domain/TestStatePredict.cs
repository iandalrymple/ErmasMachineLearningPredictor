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
using Predictor.PredictingEnginePython.Implementations;

namespace Predictor.Testing.Domain;

public class TestStatePredict
{
    private readonly IConfiguration _config;
    private readonly ILogger<PredictingEnginePythonImpl> _logger;

    public TestStatePredict()
    {
        _config = ConfigurationSingleton.Instance;

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<PredictingEnginePythonImpl>();
    }

    [Theory]
    [InlineData(2024, 5, 5, "Utica")]
    [InlineData(2024, 4, 1, "Utica")]
    [InlineData(2024, 6, 15, "Utica")]
    [InlineData(2024, 7, 4, "Utica")]
    [InlineData(2024, 5, 5, "Warren")]
    [InlineData(2024, 4, 1, "Warren")]
    [InlineData(2024, 6, 15, "Warren")]
    [InlineData(2024, 7, 4, "Warren")]
    public async Task TestExecute_Happy(int year, int month, int day, string storeName)
    {
        // Arrange
        var rawWeatherString = Properties.Resources.WeatherData_05152024;
        var rawWeatherModel = JsonConvert.DeserializeObject<StateWeatherResultModel>(rawWeatherString);
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.Aggregate,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals(storeName, StringComparison.OrdinalIgnoreCase)),
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
        var aggregate = new StateAggregate(new RetrieveHolidaysMock());
        await aggregate.Execute(container);

        var pythonEngine = new PredictingEnginePythonImpl(
            _config["PythonExe"]!,
            _config["PythonWorkingDirectory"]!,
            _config.GetSection("PythonArgs").Get<string[]>()!,
            _logger);

        var sut = new StatePredict(pythonEngine);

        // Act
        await sut.Execute(container);

        // Assert
        Assert.Equal(PredictorFsmStates.Predict + 1, container.CurrentState);
        Assert.NotNull(container.StateResults.StatePredictResults);
    }

    [Fact]
    public async Task TestPrintCsvForFeatures()
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