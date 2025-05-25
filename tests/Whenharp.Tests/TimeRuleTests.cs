using Whenharp.Rules;

namespace Whenharp.Tests;
public class TimeRuleTests
{
    [TestCase("Always", true)]
    public void AlwaysRule_ShouldAlwaysReturnTrue(string input, bool expected)
    {
        var rule = TimeRule.Parse(input);
        Assert.That(rule.IsMatch(DateTime.Now), Is.True);
        Assert.That(rule.IsMatch(DateTime.Now), Is.EqualTo(expected));
    }

    [TestCase("Never", false)]
    public void NeverRule_ShouldAlwaysReturnFalse(string input, bool expected)
    {
        var rule = TimeRule.Parse(input);
        Assert.That(rule.IsMatch(DateTime.Now), Is.False);
        Assert.That(rule.IsMatch(DateTime.Now), Is.EqualTo(expected));
    }

    [Test]
    public void RangeRule_ShouldReturnTrue_WithinRange()
    {
        var input = "From 2025-04-23T12:00 to 2025-04-23T17:00";
        var rule = TimeRule.Parse(input);

        var insideDate = new DateTime(2025, 04, 23, 13, 0, 0);
        var outsideDateBefore = new DateTime(2025, 04, 23, 11, 59, 59);
        var outsideDateAfter = new DateTime(2025, 04, 23, 17, 0, 1);

        Assert.Multiple(() =>
        {
            Assert.That(rule.IsMatch(insideDate), Is.True);
            Assert.That(rule.IsMatch(outsideDateBefore), Is.False);
            Assert.That(rule.IsMatch(outsideDateAfter), Is.False);
        });
    }

    [Test]
    public void RangeRule_ShouldThrow_ForInvalidRange()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new RangeRule(
                new DateTime(2025, 04, 23, 17, 0, 0),
                new DateTime(2025, 04, 23, 12, 0, 0));
        });
    }

    [TestCase("EveryWeekend", DayOfWeek.Saturday, true)]
    [TestCase("EveryWeekend", DayOfWeek.Sunday, true)]
    [TestCase("EveryWeekend", DayOfWeek.Friday, false)]
    public void RecurringRule_EveryWeekend_DayMatch(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 19);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.IsMatch(testDate), Is.EqualTo(expected));
    }

    [Test]
    public void RecurringRule_EveryWeekend_WithTime_ShouldMatch()
    {
        var input = "EveryWeekend from 12:00 to 17:00";
        var rule = TimeRule.Parse(input);

        var saturdayInRange = new DateTime(2025, 04, 19, 13, 0, 0);
        var saturdayBefore = new DateTime(2025, 04, 19, 11, 59, 0);
        var saturdayAfter = new DateTime(2025, 04, 19, 17, 1, 0);
        var fridayInRange = new DateTime(2025, 04, 18, 13, 0, 0);

        Assert.Multiple(() =>
        {
            Assert.That(rule.IsMatch(saturdayInRange), Is.True);
            Assert.That(rule.IsMatch(saturdayBefore), Is.False);
            Assert.That(rule.IsMatch(saturdayAfter), Is.False);
            Assert.That(rule.IsMatch(fridayInRange), Is.False);
        });
    }

    [TestCase("EveryMonday", DayOfWeek.Monday, true)]
    [TestCase("EveryMonday", DayOfWeek.Tuesday, false)]
    public void RecurringRule_EveryMonday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 21);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.IsMatch(testDate), Is.EqualTo(expected));
    }

    [Test]
    public void RecurringRule_EveryMonday_WithTime_ShouldMatchCorrectly()
    {
        var input = "EveryMonday from 12:00 to 17:00";
        var rule = TimeRule.Parse(input);

        var mondayInRange = new DateTime(2025, 04, 21, 13, 0, 0);
        var mondayBefore = new DateTime(2025, 04, 21, 11, 59, 59);
        var mondayAfter = new DateTime(2025, 04, 21, 17, 0, 1);
        var tuesdayInRange = new DateTime(2025, 04, 22, 13, 0, 0);

        Assert.Multiple(() =>
        {
            Assert.That(rule.IsMatch(mondayInRange), Is.True);
            Assert.That(rule.IsMatch(mondayBefore), Is.False);
            Assert.That(rule.IsMatch(mondayAfter), Is.False);
            Assert.That(rule.IsMatch(tuesdayInRange), Is.False);
        });
    }

    [Test]
    public void Parse_ShouldThrow_OnInvalidFormat()
    {
        var invalidInputs = new[]
        {
            "",
            "InvalidText",
            "From 2025-04-23T12:00 until 2025-04-23T17:00",
            "EveryFunday",
            "EveryMonday from 25:00 to 26:00"
        };

        foreach (var input in invalidInputs)
        {
            Assert.Throws<ArgumentException>(() => TimeRule.Parse(input));
        }
    }
}
