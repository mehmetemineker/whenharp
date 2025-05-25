namespace WhenSharp.Rules;
public class NeverRule : TimeRule
{
    public override bool IsMatch(DateTime dateTime) => false;
}
