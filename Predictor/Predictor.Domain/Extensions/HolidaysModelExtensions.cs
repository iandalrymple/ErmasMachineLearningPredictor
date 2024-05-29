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
}