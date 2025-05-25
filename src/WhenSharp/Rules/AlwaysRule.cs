namespace WhenSharp.Rules;
public class AlwaysRule : TimeRule
{
    public override bool IsMatch(DateTime dateTime) => true;
}
