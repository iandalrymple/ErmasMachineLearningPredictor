﻿using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;
using Predictor.Domain.System;
using Predictor.RetrieveOwmWeather.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.Domain
{
    public class TestStateAggregate
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoggingDecoratorRetrieveWeather> _logger;

        public TestStateAggregate()
        {
            _config = ConfigurationSingleton.Instance;

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory!.CreateLogger<LoggingDecoratorRetrieveWeather>();
        }

        [Fact]
        public async Task TestExecute_Happy()
        {
            // Arrange
            var rawWeatherString = Properties.Resources.WeatherData_05152024;
            var rawWeatherModel = JsonConvert.DeserializeObject<StateWeatherResultModel>(rawWeatherString);
            var dateToCheck = new DateTime(year: 2024, month: 5, day: 1);
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
            var sut = new StateAggregate();

            // Act
            await sut.Execute(container);

            // Assert
            Assert.Equal(PredictorFsmStates.Aggregate + 1, container.CurrentState);
            Assert.NotNull(container.StateResults.StateWeatherResults);
            Assert.Equal(4, container.StateResults.StateWeatherResults.WeatherAtTimes.Count);
        }
    }
}
