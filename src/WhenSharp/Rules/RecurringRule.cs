namespace WhenSharp.Rules;
public class RecurringRule(string day, TimeSpan? fromTime, TimeSpan? toTime) : TimeRule
{
    public string Day { get; } = day;
    public TimeSpan? FromTime { get; } = fromTime;
    public TimeSpan? ToTime { get; } = toTime;

    public override bool IsMatch(DateTime dateTime)
    {
        bool dayMatches = Day.ToLowerInvariant() switch
        {
            "weekend" => dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday,
            "monday" => dateTime.DayOfWeek == DayOfWeek.Monday,
            "tuesday" => dateTime.DayOfWeek == DayOfWeek.Tuesday,
            "wednesday" => dateTime.DayOfWeek == DayOfWeek.Wednesday,
            "thursday" => dateTime.DayOfWeek == DayOfWeek.Thursday,
            "friday" => dateTime.DayOfWeek == DayOfWeek.Friday,
            "saturday" => dateTime.DayOfWeek == DayOfWeek.Saturday,
            "sunday" => dateTime.DayOfWeek == DayOfWeek.Sunday,
            _ => false
        };

        if (!dayMatches)
            return false;

        if (FromTime.HasValue && ToTime.HasValue)
        {
            var currentTime = dateTime.TimeOfDay;
            return currentTime >= FromTime && currentTime <= ToTime;
        }

        return true;
    }
}
