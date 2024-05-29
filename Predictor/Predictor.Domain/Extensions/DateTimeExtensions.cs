namespace Predictor.Domain.Extensions;

public static class DateTimeExtensions
{
    public static DateTime DateTimeAtCertainHour(this DateTime d, int hour)
    {
        return new DateTime(
            year: d.Year,
            month: d.Month,
            day: d.Day,
            hour: hour,
            minute: 0,
            second: 0);
    }

    public static uint MinutesIntoDayForCertainDateTime(this DateTime d)
    {
        var span = d.TimeOfDay.TotalMinutes;
        return Convert.ToUInt32(span);
    }

    public static bool EqualToDateOnly(this DateTime dateTime, DateOnly dateOnly)
    {
        return dateTime.Year == dateOnly.Year && dateTime.Month == dateOnly.Month && dateTime.Day == dateOnly.Day;
    }
}