using Predictor.Domain.Models;

namespace Predictor.Domain.Abstractions;

public interface IHolidayRetriever
{
    public Task<List<HolidaysModel>?> GetHolidays(int year);
}