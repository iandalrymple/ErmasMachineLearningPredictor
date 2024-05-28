using Newtonsoft.Json;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.RetrieveHolidays.Implementations;

public class RetrieveHolidays : IHolidayRetriever
{
    private readonly Holidays.CoreLibrary.Implementations.Holidays _holidayGetter = new();

    public async Task<List<HolidaysModel>?> GetHolidays(int year)
    {
        var result = await _holidayGetter.GetHolidays(year, "US");
        var serialized = JsonConvert.SerializeObject(result);
        var converted = JsonConvert.DeserializeObject<List<HolidaysModel>>(serialized);
        return converted;
    }
}