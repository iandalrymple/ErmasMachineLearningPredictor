using Predictor.Domain.Exceptions;
using Predictor.Domain.Models;

namespace Predictor.Domain.Extensions;

public static class HolidaysModelExtensions
{
    public static bool IsDateMemorialDay(this HolidaysModel inModel, DateTime dt)
    {
        if (string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly(inModel.Date);
        return isToday && inModel.Name.Contains("Memorial", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsIndependenceDay(this HolidaysModel inModel, DateTime dt)
    {
        if (string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly(inModel.Date);
        return isToday && inModel.Name.Contains("Independence", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsLaborDay(this HolidaysModel inModel, DateTime dt)
    {
        if (string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly(inModel.Date);
        return isToday && (inModel.Name.Contains("Labour", StringComparison.OrdinalIgnoreCase)
            || inModel.Name.Contains("Labor", StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsColumbusDay(this HolidaysModel inModel, DateTime dt)
    {
        if (string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly(inModel.Date);
        return isToday && inModel.Name.Contains("Columbus", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsGoodFriday(this HolidaysModel inModel, DateTime dt)
    {
        if (string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly(inModel.Date);
        return isToday && inModel.Name.Contains("Friday", StringComparison.OrdinalIgnoreCase);
    }

    public static DateTime? CalculateEaster(IEnumerable<HolidaysModel>? allHolidays)
    {
        if (allHolidays == null)
        {
            return null;
        }

        var goodFriday = allHolidays.Where(x =>  !string.IsNullOrEmpty(x.Name) && x.Name.Contains("Friday", StringComparison.OrdinalIgnoreCase)).ToList();
        if (goodFriday.Count == 0)
        {
            return null;
        }

        var fridayDateOnly = goodFriday[0].Date;
        var fridayDateTime = new DateTime(year: fridayDateOnly.Year, month: fridayDateOnly.Month, day: fridayDateOnly.Day);
        var easterDate = fridayDateTime.AddDays(2);
        return easterDate;
    }

    public static bool IsEaster(IEnumerable<HolidaysModel>? allHolidays, DateTime dt)
    {
        var easter = CalculateEaster(allHolidays) ?? throw new HolidayException("Easter was not found.");
        return easter.Year == dt.Year && easter.Month == dt.Month && easter.Day == dt.Day;
    }

    public static bool IsMothersDay(DateTime dt)
    {
        const int mothersMonth = 5;
        if (dt.Month != mothersMonth)
        {
            return false;
        }
        var dayFinder = FindDay(dt.Year, mothersMonth, DayOfWeek.Sunday, 2);
        return dt.Day == dayFinder;
    }

    public static bool IsFathersDay(DateTime dt)
    {
        const int fathersMonth = 6;
        if (dt.Month != fathersMonth)
        {
            return false;
        }
        var dayFinder = FindDay(dt.Year, fathersMonth, DayOfWeek.Sunday, 3);
        return dt.Day == dayFinder;
    }

    public static bool IsCincoDeMayo(DateTime dt)
    {
        return dt is { Month: 5, Day: 5 };
    }

    private static int FindDay(int year, int month, DayOfWeek day, int occurence)
    {
        if (occurence is <= 0 or > 5)
        {
            throw new HolidayException("Occurence is invalid finding day.");
        }
        var firstDayOfMonth = new DateTime(year, month, 1);

        // Subtract first day of the month with the required day of the week 
        var daysNeeded = (int)day - (int)firstDayOfMonth.DayOfWeek;

        // If it is less than zero we need to get the next week day (add 7 days)
        if (daysNeeded < 0)
        {
            daysNeeded = daysNeeded + 7;
        }

        //nDayOfWeek is zero index based; multiply by the occurrence to get the day
        var resultedDay = (daysNeeded + 1) + (7 * (occurence - 1));

        if (resultedDay > (firstDayOfMonth.AddMonths(1) - firstDayOfMonth).Days)
        {
            throw new HolidayException(string.Format("No {0} occurence(s) of {1} in the required month", occurence,
                day.ToString()));
        }

        return resultedDay;
    }
}