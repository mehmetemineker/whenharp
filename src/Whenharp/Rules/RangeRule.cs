﻿namespace Whenharp.Rules;
public class RangeRule : When
{
    public DateTime From { get; }
    public DateTime To { get; }

    public RangeRule(DateTime from, DateTime to)
    {
        if (to < from) throw new ArgumentException("'To' date must be after 'From' date.");
        From = from;
        To = to;
    }

    public override bool Match(DateTime dateTime)
    {
        return dateTime >= From && dateTime <= To;
    }
}
