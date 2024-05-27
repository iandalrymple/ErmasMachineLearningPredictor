using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateRetrieveCurrentSales : IFsmState
{
    private readonly IRetrieveSales _retrieveSales;

    public StateRetrieveCurrentSales(IRetrieveSales retrieveSales)
    {
        State = PredictorFsmStates.CurrentSalesRetrieve;
        _retrieveSales = retrieveSales;
    }

    public PredictorFsmStates State { get; init; }

    public async Task Execute(FsmStatefulContainer container)
    {
        var sales = await _retrieveSales.Retrieve(DateTime.Now, container.StoreLocation.Name);
        container.StateResults.StateCurrentSalesResults = new StateCurrentSalesResultModel
        {
            SalesAtThree = sales
        };
        container.CurrentState++;
    }
}