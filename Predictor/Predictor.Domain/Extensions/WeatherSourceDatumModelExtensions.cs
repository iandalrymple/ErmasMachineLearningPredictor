using Predictor.Domain.Models;

namespace Predictor.Domain.Extensions;

public static class WeatherSourceDatumModelExtensions
{
    public static bool IsRaining(this Datum data)
    {
        if (data.Weather == null)
        {
            return false;
        }

        foreach (var w in data.Weather)
        {
            if (w.Id is >= 200 and <= 232 
                or >= 300 and <= 321 
                or >= 500 and <= 531)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsSnowing(this Datum data)
    {
        if (data.Weather == null)
        {
            return false;
        }

        foreach (var w in data.Weather)
        {
            if (w.Id is >= 600 and <= 622)
            {
                return true;
            }
        }

        return false;
    }
}