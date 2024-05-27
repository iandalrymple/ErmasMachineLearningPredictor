﻿using Predictor.Domain.Abstractions;
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
        var result = new StateAggregateResultModel
        {
            Sales_Three_Pm = container.StateResults.StateCurrentSalesResults.SalesAtThree,
            TotalSalesDayBefore = container.StateResults.StateHistoricSalesResults.SalesDayBefore,
            TotalSalesTwoDaysBefore = container.StateResults.StateHistoricSalesResults.SalesTwoDaysBefore,
            First_Order_Minutes_In_Day = container.StateResults.StateCurrentSalesResults.FirstOrderMinutesInDay,
            Last_Order_Minutes_In_Day = container.StateResults.StateCurrentSalesResults.LastOrderMinutesInDay,

            WeekDayNumberSundayAsZero = (int)container.DateToCheck.DayOfWeek,
            DayOfMonth = container.DateToCheck.Day,
            Month = container.DateToCheck.Month,
            Year = container.DateToCheck.Year,
            JulianDay = container.DateToCheck.DayOfYear,



        };

        // Move onto next state.
        container.CurrentState++;

        // Bounce back a completed task.
        return Task.CompletedTask;
    }
}