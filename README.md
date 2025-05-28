# Whenharp

![Tests](https://github.com/mehmetemineker/whenharp/actions/workflows/dotnet-tests.yml/badge.svg)
[![publish Whenharp to nuget](https://github.com/mehmetemineker/whenharp/actions/workflows/publish.yml/badge.svg)](https://github.com/mehmetemineker/whenharp/actions/workflows/publish.yml)
[![Whenharp on NuGet](https://img.shields.io/nuget/v/Whenharp?label=Whenharp)](https://www.nuget.org/packages/Whenharp/)

**Whenharp** is a lightweight and powerful .NET library designed to parse human-readable time rules and determine whether a given `DateTime` matches those rules.

## 🚀 Features

- 🕒 Simple and readable time rule syntax
- ✅ Basic rules like `Always` and `Never`
- 📅 Specific date ranges: `From 2025-04-23T12:00 to 2025-04-23T17:00`
- 🔁 Recurring day rules: `EveryMonday`, `EveryWeekend`
- ⏰ Daily time intervals combined with day rules: `EveryMonday from 12:00 to 17:00`
- 💥 Throws detailed `ArgumentException` for invalid inputs

## 📦 Installation

Install via NuGet:

```bash
dotnet add package Whenharp
```

## 📌 Usage Examples

### ✅ Basic Usage
```cs
using WhenSharp;

// Parse a time rule
var rule = When.Parse("EveryMonday from 09:00 to 17:00");

// Check if a given DateTime matches the rule
bool isMatch = rule.Match(DateTime.Now);

Console.WriteLine($"Is match: {isMatch}");
```

## 📅 Supported Rule Formats
### 🔹 Always
```cs
var rule = When.Parse("Always");
rule.Match(DateTime.Now); // always true
```
### 🔹 Never
```cs
var rule = When.Parse("Never");
rule.Match(DateTime.Now); // always false
```
### 🔹 From a Specific DateTime to Another
```cs
var rule = When.Parse("From 2025-04-23T08:30 to 2025-04-23T17:45");
rule.Match(new DateTime(2025, 4, 23, 9, 0, 0)); // true
rule.Match(new DateTime(2025, 4, 24, 9, 0, 0)); // false
```
### 🔹 Recurring Rule: Every Day of the Week
```cs
var rule = When.Parse("EveryMonday");
rule.Match(new DateTime(2025, 5, 5)); // true if Monday
```
### 🔹 Recurring with Time Range
```cs
var rule = When.Parse("EveryFriday from 14:00 to 18:00");
rule.Match(new DateTime(2025, 6, 6, 15, 30, 0)); // true
rule.Match(new DateTime(2025, 6, 6, 19, 0, 0));  // false
```
### 🔹 EveryWeekend (Saturday & Sunday)
```cs
var rule = When.Parse("EveryWeekend");
rule.Match(new DateTime(2025, 5, 25)); // true if Sunday
```
### 🔹 EveryWeekend with Time Range
```cs
var rule = When.Parse("EveryWeekend from 10:00 to 16:00");
rule.Match(new DateTime(2025, 5, 24, 12, 0, 0)); // true if Saturday
```

## ⚠️ Invalid Rule Handling
```cs
try
{
    var rule = When.Parse("EveryBlursday");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid rule: {ex.Message}");
}
```

## Attributions

The icon used for the package belongs to [Freepik](https://www.flaticon.com/free-icon/flux_6642118).
