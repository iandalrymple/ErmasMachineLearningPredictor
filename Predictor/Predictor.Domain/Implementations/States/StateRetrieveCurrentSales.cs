using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateRetrieveCurrentSales : IFsmState
{
    private readonly IRetrieveSales<StateCurrentSalesResultModel> _retrieveSales;

    public StateRetrieveCurrentSales(IRetrieveSales<StateCurrentSalesResultModel> retrieveSales)
    {
        State = PredictorFsmStates.CurrentSalesRetrieve;
        _retrieveSales = retrieveSales;
    }

    public PredictorFsmStates State { get; init; }

    public async Task Execute(FsmStatefulContainer container)
    {
        var result = await _retrieveSales.Retrieve(container.DateToCheck, container.StoreLocation.Name);
        container.StateResults.StateCurrentSalesResults = new StateCurrentSalesResultModel
        {
            SalesAtThree = result.SalesAtThree,
            FirstOrderMinutesInDay = result.FirstOrderMinutesInDay,
            LastOrderMinutesInDay = result.LastOrderMinutesInDay
        };
        container.CurrentState++;
    }
}