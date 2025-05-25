namespace WhenSharp.Rules;
public class RangeRule : TimeRule
{
    public DateTime From { get; }
    public DateTime To { get; }

    public RangeRule(DateTime from, DateTime to)
    {
        if (to < from) throw new ArgumentException("'To' date must be after 'From' date.");
        From = from;
        To = to;
    }

    public override bool IsMatch(DateTime dateTime)
    {
        return dateTime >= From && dateTime <= To;
    }
}
