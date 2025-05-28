namespace Whenharp.Rules;
public class NeverRule : When
{
    public override bool Match(DateTime dateTime) => false;
}
