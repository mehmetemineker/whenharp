using System.Globalization;
using System.Text.RegularExpressions;

namespace WhenSharp.Rules;
public abstract class TimeRule
{
    /// <summary>
    /// Verilen datetime, kurala uyuyorsa true döner.
    /// </summary>
    public abstract bool IsMatch(DateTime dateTime);

    /// <summary>
    /// Kural ifadesini parse edip uygun TimeRule nesnesini döner.
    /// </summary>
    public static TimeRule Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or empty.");

        input = input.Trim();

        // Always
        if (string.Equals(input, "Always", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input, "Allways", StringComparison.OrdinalIgnoreCase)) // Yazım hatası için destek
        {
            return new AlwaysRule();
        }

        // Never
        if (string.Equals(input, "Never", StringComparison.OrdinalIgnoreCase))
        {
            return new NeverRule();
        }

        // From ... to ...
        var fromToPattern = @"^From\s+(\d{4}-\d{2}-\d{2}T\d{2}:\d{2})\s+to\s+(\d{4}-\d{2}-\d{2}T\d{2}:\d{2})$";
        var fromToMatch = Regex.Match(input, fromToPattern, RegexOptions.IgnoreCase);
        if (fromToMatch.Success)
        {
            if (DateTime.TryParseExact(fromToMatch.Groups[1].Value, "yyyy-MM-dd'T'HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from) &&
                DateTime.TryParseExact(fromToMatch.Groups[2].Value, "yyyy-MM-dd'T'HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
            {
                return new RangeRule(from, to);
            }
            else
            {
                throw new ArgumentException("Invalid date format in 'From ... to ...' pattern.");
            }
        }

        // EveryWeekend [from HH:mm to HH:mm]
        var everyWeekendPattern = @"^EveryWeekend(?:\s+from\s+(\d{2}:\d{2})\s+to\s+(\d{2}:\d{2}))?$";
        var weekendMatch = Regex.Match(input, everyWeekendPattern, RegexOptions.IgnoreCase);
        if (weekendMatch.Success)
        {
            TimeSpan? fromTime = null;
            TimeSpan? toTime = null;
            if (weekendMatch.Groups[1].Success && weekendMatch.Groups[2].Success)
            {
                if (TimeSpan.TryParseExact(weekendMatch.Groups[1].Value, @"hh\:mm", CultureInfo.InvariantCulture, out var ft) &&
                    TimeSpan.TryParseExact(weekendMatch.Groups[2].Value, @"hh\:mm", CultureInfo.InvariantCulture, out var tt))
                {
                    fromTime = ft;
                    toTime = tt;
                }
                else
                {
                    throw new ArgumentException("Invalid time format in 'EveryWeekend from HH:mm to HH:mm'.");
                }
            }
            return new RecurringRule("Weekend", fromTime, toTime);
        }

        // EveryMonday gibi tek gün
        var everyDayPattern = @"^Every(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$";
        var everyDayMatch = Regex.Match(input, everyDayPattern, RegexOptions.IgnoreCase);
        if (everyDayMatch.Success)
        {
            return new RecurringRule(everyDayMatch.Groups[1].Value, null, null);
        }

        // EveryMonday from HH:mm to HH:mm
        var everyDayWithTimePattern = @"^Every(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)\s+from\s+(\d{2}:\d{2})\s+to\s+(\d{2}:\d{2})$";
        var everyDayWithTimeMatch = Regex.Match(input, everyDayWithTimePattern, RegexOptions.IgnoreCase);
        if (everyDayWithTimeMatch.Success)
        {
            if (TimeSpan.TryParseExact(everyDayWithTimeMatch.Groups[2].Value, @"hh\:mm", CultureInfo.InvariantCulture, out var fromTime) &&
                TimeSpan.TryParseExact(everyDayWithTimeMatch.Groups[3].Value, @"hh\:mm", CultureInfo.InvariantCulture, out var toTime))
            {
                return new RecurringRule(everyDayWithTimeMatch.Groups[1].Value, fromTime, toTime);
            }
            else
            {
                throw new ArgumentException("Invalid time format in 'EveryDay from HH:mm to HH:mm'.");
            }
        }

        throw new ArgumentException($"Unsupported time rule format: '{input}'");
    }
}
