using Predictor.Domain.Abstractions;
using Predictor.Domain.Extensions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateAggregate : IFsmState
{
    private readonly IHolidayRetriever _holidayRetriever;

    public StateAggregate(IHolidayRetriever retriever)
    {
        State = PredictorFsmStates.Aggregate;
        _holidayRetriever = retriever;
    }
    
    public PredictorFsmStates State { get; init; }

    public async Task Execute(FsmStatefulContainer container)
    {
        // Perform null checks.
        if (container.StateResults.StateCurrentSalesResults == null ||
            container.StateResults.StateHistoricSalesResults == null ||
            container.StateResults.StateWeatherResults == null)
        {
            throw new ArgumentNullException(nameof(container.StateResults));
        }

        // Grab the holiday information.
        var holidays = await _holidayRetriever.GetHolidays(container.DateToCheck.Year);
        if (holidays == null)
        {
            container.ApplicableError = new ErrorModel
            {
                Message = "Holidays returned null",
                StateErrorOccurredIn = container.CurrentState,
                Exception = null
            };
            container.CurrentState = PredictorFsmStates.Error;
            return;
        }
        var holidayNameStringList = GetHoliday(container.DateToCheck, holidays);

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

            isMemorialDay = holidays.Exists(x => x.IsDateMemorialDay(container.DateToCheck))


        };

        // Move onto next state.
        container.CurrentState++;
    }

    private static IEnumerable<string> GetHoliday(DateTime dt, IEnumerable<HolidaysModel> holidaysModels)
    {
        var todayHoliday = holidaysModels
            .Where(day => day.Date != null && dt.EqualToDateOnly((DateOnly)day.Date))
            .Where(x => x.Name != null);
        return todayHoliday.Select(x => x.Name!).ToList();
    }


}