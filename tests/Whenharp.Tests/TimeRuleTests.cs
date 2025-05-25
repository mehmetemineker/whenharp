using Whenharp.Rules;

namespace Whenharp.Tests;
public class TimeRuleTests
{
    [TestCase("Always", true)]
    public void AlwaysRule_ShouldAlwaysReturnTrue(string input, bool expected)
    {
        var rule = TimeRule.Parse(input);
        Assert.That(rule.Match(DateTime.Now), Is.True);
        Assert.That(rule.Match(DateTime.Now), Is.EqualTo(expected));
    }

    [TestCase("Never", false)]
    public void NeverRule_ShouldAlwaysReturnFalse(string input, bool expected)
    {
        var rule = TimeRule.Parse(input);
        Assert.That(rule.Match(DateTime.Now), Is.False);
        Assert.That(rule.Match(DateTime.Now), Is.EqualTo(expected));
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
            Assert.That(rule.Match(insideDate), Is.True);
            Assert.That(rule.Match(outsideDateBefore), Is.False);
            Assert.That(rule.Match(outsideDateAfter), Is.False);
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

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
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
            Assert.That(rule.Match(saturdayInRange), Is.True);
            Assert.That(rule.Match(saturdayBefore), Is.False);
            Assert.That(rule.Match(saturdayAfter), Is.False);
            Assert.That(rule.Match(fridayInRange), Is.False);
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

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EveryTuesday", DayOfWeek.Tuesday, true)]
    [TestCase("EveryTuesday", DayOfWeek.Wednesday, false)]
    public void RecurringRule_EveryTuesday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 21);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EveryWednesday", DayOfWeek.Wednesday, true)]
    [TestCase("EveryWednesday", DayOfWeek.Thursday, false)]
    public void RecurringRule_EveryWednesday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 23);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EveryThursday", DayOfWeek.Thursday, true)]
    [TestCase("EveryThursday", DayOfWeek.Friday, false)]
    public void RecurringRule_EveryThursday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 24);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EveryFriday", DayOfWeek.Friday, true)]
    [TestCase("EveryFriday", DayOfWeek.Saturday, false)]
    public void RecurringRule_EveryFriday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 25);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EverySaturday", DayOfWeek.Saturday, true)]
    [TestCase("EverySaturday", DayOfWeek.Sunday, false)]
    public void RecurringRule_EverySaturday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 26);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
    }

    [TestCase("EverySunday", DayOfWeek.Sunday, true)]
    [TestCase("EverySunday", DayOfWeek.Monday, false)]
    public void RecurringRule_EverySunday_ShouldMatchCorrectDay(string input, DayOfWeek day, bool expected)
    {
        var rule = TimeRule.Parse(input);
        var testDate = new DateTime(2025, 04, 27);

        while (testDate.DayOfWeek != day)
        {
            testDate = testDate.AddDays(1);
        }

        Assert.That(rule.Match(testDate), Is.EqualTo(expected));
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
            Assert.That(rule.Match(mondayInRange), Is.True);
            Assert.That(rule.Match(mondayBefore), Is.False);
            Assert.That(rule.Match(mondayAfter), Is.False);
            Assert.That(rule.Match(tuesdayInRange), Is.False);
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
