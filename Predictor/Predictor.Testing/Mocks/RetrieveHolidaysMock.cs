using Newtonsoft.Json;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.Testing.Mocks;

internal class RetrieveHolidaysMock : IHolidayRetriever
{
    public async Task<List<HolidaysModel>?> GetHolidays(int year)
    {
        var holidays = JsonConvert.DeserializeObject<List<HolidaysModel>>(Properties.Resources.Holidays2024);
        await Task.Delay(1_000);
        return holidays;
    }
}