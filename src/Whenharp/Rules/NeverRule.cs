namespace Whenharp.Rules;
public class NeverRule : TimeRule
{
    public override bool Match(DateTime dateTime) => false;
}
