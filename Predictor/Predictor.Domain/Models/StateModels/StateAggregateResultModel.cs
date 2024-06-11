// ReSharper disable InconsistentNaming
// Disabled to match the names being used in the receiving Python application.

namespace Predictor.Domain.Models.StateModels;

public sealed class StateAggregateResultModel
{
    public decimal Sales_Three_Pm { get; set; }
    public decimal TotalSalesDayBefore { get; set; }
    public decimal TotalSalesTwoDaysBefore { get; set; }

    public uint First_Order_Minutes_In_Day { get; set; }
    public uint Last_Order_Minutes_In_Day { get; set; }

    public int WeekDayNumber { get; set; }
    public int DayOfMonth { get; set; }    
    public int Month { get; set; }
    public int Year { get; set; }
    public int JulianDay { get; set; }
    public bool isMemorialDay { get; set; }
    public bool isIndependenceDay { get; set; }
    public bool isLaborDay { get; set; }
    public bool isColumbusDay { get; set; }
    public bool isCincoDeMayo { get; set; }
    public bool isOpeningDay { get; set; }
    public bool isClosingDay { get; set; }
    public bool isEaster { get; set; }
    public bool isGoodFriday { get; set; }
    public bool isMothersDay { get; set; }
    public bool isFathersDay { get; set; }

    public double TempNoon { get; set; }
    public double FeelsLikeNoon { get; set; }
    public double PressureNoon { get; set; }
    public double HumidityNoon { get; set; }
    public double DewPointNoon { get; set; }
    public double UviNoon { get; set; }
    public double CloudsNoon { get; set; }
    public double VisibilityNoon { get; set; }
    public double WindSpeedNoon { get; set; }
    public double WindGustNoon { get; set; }
    public double WindDegNoon { get; set; }
    public bool Noon_Raining { get; set; }
    public bool Noon_Snowing { get; set; }

    public double TempThree { get; set; }
    public double FeelsLikeThree { get; set; }
    public double PressureThree { get; set; }
    public double HumidityThree { get; set; }
    public double DewPointThree { get; set; }
    public double UviThree { get; set; }
    public double CloudsThree { get; set; }
    public double VisibilityThree { get; set; }
    public double WindSpeedThree { get; set; }
    public double WindGustThree { get; set; }
    public double WindDegThree { get; set; }
    public bool Three_Raining { get; set; }
    public bool Three_Snowing { get; set; }

    public double TempSix { get; set; }
    public bool Six_Raining { get; set; }
    public bool Six_Snowing { get; set; }

    public double TempNine { get; set; }
    public bool Nine_Raining { get; set; }
    public bool Nine_Snowing { get; set; }

    public bool IsPopulated { get; set; }

    public string? CreateCsvRows()
    {
        if (!IsPopulated)
        {
            return null;
        }

        var returnString = $"{Header}{Environment.NewLine}" +

                           $"{Sales_Three_Pm}," +

                           $"{TempNoon}," +
                           $"{FeelsLikeNoon}," +
                           $"{PressureNoon}," +
                           $"{HumidityNoon}," +
                           $"{DewPointNoon}," +
                           $"{UviNoon}," +
                           $"{CloudsNoon}," +
                           $"{VisibilityNoon}," +
                           $"{WindSpeedNoon}," +
                           $"{WindGustNoon}," +
                           $"{WindDegNoon}," +

                           $"{TempThree}," +
                           $"{FeelsLikeThree}," +
                           $"{PressureThree}," +
                           $"{HumidityThree}," +
                           $"{DewPointThree}," +
                           $"{UviThree}," +
                           $"{CloudsThree}," +
                           $"{VisibilityThree}," +
                           $"{WindSpeedThree}," +
                           $"{WindGustThree}," +
                           $"{WindDegThree}," +

                           $"{TempSix}," +
                           $"{TempNine}," +
                           $"{Noon_Raining}," +
                           $"{Three_Raining}," +
                           $"{Six_Raining}," +
                           $"{Nine_Raining}," +
                           $"{Noon_Snowing}," +
                           $"{Three_Snowing}," +
                           $"{Six_Snowing}," +
                           $"{Nine_Snowing}," +

                           $"{First_Order_Minutes_In_Day}," +
                           $"{Last_Order_Minutes_In_Day}," +
                           $"{WeekDayNumber}," +
                           $"{DayOfMonth}," +
                           $"{Month}," +
                           $"{Year}," +
                           $"{JulianDay}," +
                           $"{isMemorialDay}," +
                           $"{isIndependenceDay}," +
                           $"{isLaborDay}," +
                           $"{isColumbusDay}," +
                           $"{isCincoDeMayo}," +
                           $"{isOpeningDay}," +
                           $"{isClosingDay}," +
                           $"{isEaster}," +
                           $"{isGoodFriday}," +
                           $"{isMothersDay}," +
                           $"{isFathersDay}," +
                           $"{TotalSalesDayBefore}," +
                           $"{TotalSalesTwoDaysBefore}";

        return returnString;
    }

    public const string Header = nameof(Sales_Three_Pm) + "," +

                                 nameof(TempNoon) + "," +
                                 nameof(FeelsLikeNoon) + "," +
                                 nameof(PressureNoon) + "," +
                                 nameof(HumidityNoon) + "," +
                                 nameof(DewPointNoon) + "," +
                                 nameof(UviNoon) + "," +
                                 nameof(CloudsNoon) + "," +
                                 nameof(VisibilityNoon) + "," +
                                 nameof(WindSpeedNoon) + "," +
                                 nameof(WindGustNoon) + "," +
                                 nameof(WindDegNoon) + "," +

                                 nameof(TempThree) + "," +
                                 nameof(FeelsLikeThree) + "," +
                                 nameof(PressureThree) + "," +
                                 nameof(HumidityThree) + "," +
                                 nameof(DewPointThree) + "," +
                                 nameof(UviThree) + "," +
                                 nameof(CloudsThree) + "," +
                                 nameof(VisibilityThree) + "," +
                                 nameof(WindSpeedThree) + "," +
                                 nameof(WindGustThree) + "," +
                                 nameof(WindDegThree) + "," +

                                 nameof(TempSix) + "," +
                                 nameof(TempNine) + "," +

                                 nameof(Noon_Raining) + "," +
                                 nameof(Three_Raining) + "," +
                                 nameof(Six_Raining) + "," +
                                 nameof(Nine_Raining) + "," +

                                 nameof(Noon_Snowing) + "," +
                                 nameof(Three_Snowing) + "," +
                                 nameof(Six_Snowing) + "," +
                                 nameof(Nine_Snowing) + "," +

                                 nameof(First_Order_Minutes_In_Day) + "," +
                                 nameof(Last_Order_Minutes_In_Day) + "," +
                                 nameof(WeekDayNumber) + "," +
                                 nameof(DayOfMonth) + "," +
                                 nameof(Month) + "," +
                                 nameof(Year) + "," +
                                 nameof(JulianDay) + "," +
                                 nameof(isMemorialDay) + "," +
                                 nameof(isIndependenceDay) + "," +
                                 nameof(isLaborDay) + "," +
                                 nameof(isColumbusDay) + "," +
                                 nameof(isCincoDeMayo) + "," +
                                 nameof(isOpeningDay) + "," +
                                 nameof(isClosingDay) + "," +
                                 nameof(isEaster) + "," +
                                 nameof(isGoodFriday) + "," +
                                 nameof(isMothersDay) + "," +
                                 nameof(isFathersDay) + "," +

                                 nameof(TotalSalesDayBefore) + "," +
                                 nameof(TotalSalesTwoDaysBefore);
}