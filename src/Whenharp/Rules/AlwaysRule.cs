namespace Whenharp.Rules;
public class AlwaysRule : TimeRule
{
    public override bool Match(DateTime dateTime) => true;
}
