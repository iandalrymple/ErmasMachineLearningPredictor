using Microsoft.Extensions.Configuration;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.Domain;

public class TestStateRetrieveHistoricSales
{
    private readonly IConfiguration _config = ConfigurationSingleton.Instance;

    [Theory]
    [InlineData(2024, 4, 15)]
    [InlineData(2024, 4, 22)]
    public async Task TestExecute_Happy(int year, int month, int day)
    {
        // Arrange
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var retriever = new Predictor.RetrieveSalesSqlServer.Implementations.RetrieveSales(_config["ConnectionStringSqlExpressOne"]!);
        var sut = new StateRetrieveHistoricSales(retriever);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.HistoricSalesRetrieve,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StatesCombinedResultModel(),
            DateToCheck = dateToCheck
        };

        // Act
        await sut.Execute(container);

        // Assert
        Assert.Equal(PredictorFsmStates.HistoricSalesRetrieve + 1, container.CurrentState);
        Assert.NotNull(container.StateResults.StateHistoricSalesResults);
    }
}