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
            container.StateResults.StateWeatherResults == null ||
            !container.StateResults.StateWeatherResults.WeatherAtTimes.TryGetValue(12, out _) ||
            !container.StateResults.StateWeatherResults.WeatherAtTimes.TryGetValue(15, out _) ||
            !container.StateResults.StateWeatherResults.WeatherAtTimes.TryGetValue(18, out _) ||
            !container.StateResults.StateWeatherResults.WeatherAtTimes.TryGetValue(21, out _) ||
            container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data == null ||
            container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data == null ||
            container.StateResults.StateWeatherResults.WeatherAtTimes[18].Data == null ||
            container.StateResults.StateWeatherResults.WeatherAtTimes[21].Data == null)
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

        // Spin up the result object.
        var resultModel = new StateAggregateResultModel
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

            isMemorialDay = holidays.Exists(x => x.IsDateMemorialDay(container.DateToCheck)),
            isIndependenceDay = holidays.Exists(x => x.IsIndependenceDay(container.DateToCheck)),
            isLaborDay = holidays.Exists(x => x.IsLaborDay(container.DateToCheck)),
            isColumbusDay = holidays.Exists(x => x.IsColumbusDay(container.DateToCheck)),
            isCincoDeMayo = HolidaysModelExtensions.IsCincoDeMayo(container.DateToCheck),
            isOpeningDay = HolidaysModelExtensions.IsOpeningDay(container.DateToCheck),
            isClosingDay = HolidaysModelExtensions.IsClosingDay(container.DateToCheck),
            isEaster = HolidaysModelExtensions.IsEaster(holidays, container.DateToCheck),
            isGoodFriday = holidays.Exists(x => x.IsGoodFriday(container.DateToCheck)),
            isMothersDay = HolidaysModelExtensions.IsMothersDay(container.DateToCheck),
            isFatherDay = HolidaysModelExtensions.IsFathersDay(container.DateToCheck),

            // TODO - need to verify for nulls and more than one Data element
            TempNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Temp,
            FeelsLikeNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().FeelsLike,
            PressureNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Pressure,
            HumidityNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Humidity,
            DewPointNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().DewPoint,
            UviNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Uvi,
            CloudsNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Clouds,
            VisibilityNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().Visibility,
            WindSpeedNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().WindSpeed,
            WindGustNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().WindGust,
            WindDegNoon = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().WindDeg,
            NoonRaining = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().IsRaining(),
            NoonSnowing = container.StateResults.StateWeatherResults.WeatherAtTimes[12].Data!.First().IsSnowing(),

            TempThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Temp,
            FeelsLikeThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().FeelsLike,
            PressureThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Pressure,
            HumidityThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Humidity,
            DewPointThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().DewPoint,
            UviThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Uvi,
            CloudsThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Clouds,
            VisibilityThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().Visibility,
            WindSpeedThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().WindSpeed,
            WindGustThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().WindGust,
            WindDegThree = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().WindDeg,
            ThreeRaining = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().IsRaining(),
            ThreeSnowing = container.StateResults.StateWeatherResults.WeatherAtTimes[15].Data!.First().IsSnowing(),

            TempSix = container.StateResults.StateWeatherResults.WeatherAtTimes[18].Data!.First().Temp,
            SixRaining = container.StateResults.StateWeatherResults.WeatherAtTimes[18].Data!.First().IsRaining(),
            SixSnowing = container.StateResults.StateWeatherResults.WeatherAtTimes[18].Data!.First().IsSnowing(),

            TempNine = container.StateResults.StateWeatherResults.WeatherAtTimes[21].Data!.First().Temp,
            NineRaining = container.StateResults.StateWeatherResults.WeatherAtTimes[21].Data!.First().IsRaining(),
            NineSnowing = container.StateResults.StateWeatherResults.WeatherAtTimes[21].Data!.First().IsSnowing()
        };
        container.StateResults.StateAggregateResults = resultModel;

        // Move onto next state.
        container.CurrentState++;
    }
}