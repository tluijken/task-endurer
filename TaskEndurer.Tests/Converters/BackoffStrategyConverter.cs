using System;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace TaskEndurer.Tests.Converters;

[Binding]
internal class BackoffStrategyConverter
{
    [StepArgumentTransformation(@"(linear|exponential|fixed|fibonacci|polynomial)")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BackoffStrategy EnabledDisabledToBool(string value)
    {
        return Enum.Parse<BackoffStrategy>(value, true);
    }
}