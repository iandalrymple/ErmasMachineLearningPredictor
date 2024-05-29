using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;
using Predictor.Domain.System;
using Predictor.RetrieveOwmWeather.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Predictor.Testing.Supporting;
using Predictor.Domain.Extensions;

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

        [Theory]
        [InlineData(2024, 5, 27, true)]
        [InlineData(2024, 5, 28, false)]
        [InlineData(2024, 5, 26, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsMemorialDay(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = holidays!.Exists(x => x.IsDateMemorialDay(dateToCheck));

            // Assert
            Assert.True(result == assertResult);
        }

        [Theory]
        [InlineData(2024, 7, 4, true)]
        [InlineData(2024, 7, 5, false)]
        [InlineData(2024, 7, 3, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsIndependenceDay(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = holidays!.Exists(x => x.IsIndependenceDay(dateToCheck));

            // Assert
            Assert.True(result == assertResult);
        }

        [Theory]
        [InlineData(2024, 9, 2, true)]
        [InlineData(2024, 9, 3, false)]
        [InlineData(2024, 9, 1, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsLaborDay(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = holidays!.Exists(x => x.IsLaborDay(dateToCheck));

            // Assert
            Assert.True(result == assertResult);
        }

        [Theory]
        [InlineData(2024, 10, 14, true)]
        [InlineData(2024, 10, 15, false)]
        [InlineData(2024, 10, 13, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsColumbusDay(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = holidays!.Exists(x => x.IsColumbusDay(dateToCheck));

            // Assert
            Assert.True(result == assertResult);
        }

        [Theory]
        [InlineData(2024, 5, 5, true)]
        [InlineData(2024, 5, 6, false)]
        [InlineData(2024, 5, 4, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsCincoDeMayo(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = HolidaysModelExtensions.IsCincoDeMayo(dateToCheck);

            // Assert
            Assert.True(result == assertResult);
        }

        [Theory]
        [InlineData(2024, 3, 29, true)]
        [InlineData(2024, 3, 30, false)]
        [InlineData(2024, 3, 28, false)]
        [InlineData(2024, 12, 25, false)]
        public void TestIsGoodFriday(int year, int month, int day, bool assertResult)
        {
            // Arrange
            var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
            var dateToCheck = new DateTime(year, month, day);

            // Act
            var result = holidays!.Exists(x => x.IsGoodFriday(dateToCheck));

            // Assert
            Assert.True(result == assertResult);
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
            //var sut = new StateAggregate();

            // Act
            //await sut.Execute(container);

            // Assert
            Assert.Equal(PredictorFsmStates.Aggregate + 1, container.CurrentState);
            Assert.NotNull(container.StateResults.StateWeatherResults);
            Assert.Equal(4, container.StateResults.StateWeatherResults.WeatherAtTimes.Count);
        }
    }
}
