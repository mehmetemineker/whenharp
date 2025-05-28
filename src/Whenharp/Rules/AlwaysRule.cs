namespace Whenharp.Rules;
public class AlwaysRule : When
{
    public override bool Match(DateTime dateTime) => true;
}
