using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateAggregate : IFsmState
{
    public StateAggregate()
    {
        State = PredictorFsmStates.Aggregate;
    }
    
    public PredictorFsmStates State { get; init; }

    public Task Execute(FsmStatefulContainer container)
    {
        // Perform null checks.
        if (container.StateResults.StateCurrentSalesResults == null ||
            container.StateResults.StateHistoricSalesResults == null ||
            container.StateResults.StateWeatherResults == null)
        {
            throw new ArgumentNullException(nameof(container.StateResults));
        }

        // Spin up the result object.
        var result = new StateAggregateResultModel();
        result.Sales_Three_Pm = container.StateResults.StateCurrentSalesResults.SalesAtThree;
        result.TotalSalesDayBefore = container.StateResults.StateHistoricSalesResults.SalesDayBefore;
        result.TotalSalesTwoDaysBefore = container.StateResults.StateHistoricSalesResults.SalesTwoDaysBefore;


        // Move onto next state.
        container.CurrentState++;

        // Bounce back a completed task.
        return Task.CompletedTask;
    }
}