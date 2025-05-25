# WhenSharp

**WhenSharp** is a lightweight and powerful .NET library designed to parse human-readable time rules and determine whether a given `DateTime` matches those rules.

## 🚀 Features

- 🕒 Simple and readable time rule syntax
- ✅ Basic rules like `Always` and `Never`
- 📅 Specific date ranges: `From 2025-04-23T12:00 to 2025-04-23T17:00`
- 🔁 Recurring day rules: `EveryMonday`, `EveryWeekend`
- ⏰ Daily time intervals combined with day rules: `EveryMonday from 12:00 to 17:00`
- 💥 Throws detailed `ArgumentException` for invalid inputs
- 🧪 Unit tests powered by NUnit

## 📦 Installation

Install via NuGet:

```bash
dotnet add package WhenSharp
```

## 💡 Usage Example

```cs
using WhenSharp;

var rule = TimeRule.Parse("EveryWeekend from 12:00 to 17:00");

var now = new DateTime(2025, 4, 19, 13, 0, 0); // Saturday at 13:00

if (rule.IsMatch(now))
{
    Console.WriteLine("Rule matched!");
}
```

