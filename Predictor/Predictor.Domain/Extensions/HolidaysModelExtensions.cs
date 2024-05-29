using Predictor.Domain.Models;

namespace Predictor.Domain.Extensions;

public static class HolidaysModelExtensions
{
    public static bool IsDateMemorialDay(this HolidaysModel inModel, DateTime dt)
    {
        if (inModel.Date == null || string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly((DateOnly)inModel.Date);
        return isToday && inModel.Name.Contains("Memorial", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsIndependenceDay(this HolidaysModel inModel, DateTime dt)
    {
        if (inModel.Date == null || string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly((DateOnly)inModel.Date);
        return isToday && inModel.Name.Contains("Independence", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsLaborDay(this HolidaysModel inModel, DateTime dt)
    {
        if (inModel.Date == null || string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly((DateOnly)inModel.Date);
        return isToday && (inModel.Name.Contains("Labour", StringComparison.OrdinalIgnoreCase)
            || inModel.Name.Contains("Labor", StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsColumbusDay(this HolidaysModel inModel, DateTime dt)
    {
        if (inModel.Date == null || string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly((DateOnly)inModel.Date);
        return isToday && inModel.Name.Contains("Columbus", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsGoodFriday(this HolidaysModel inModel, DateTime dt)
    {
        if (inModel.Date == null || string.IsNullOrEmpty(inModel.Name))
        {
            return false;
        }

        var isToday = dt.EqualToDateOnly((DateOnly)inModel.Date);
        return isToday && inModel.Name.Contains("Friday", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsCincoDeMayo(DateTime dt)
    {
        return dt is { Month: 5, Day: 5 };
    }
}