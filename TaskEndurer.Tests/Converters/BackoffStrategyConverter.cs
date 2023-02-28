using System;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace TaskEndurer.Tests.Converters;

[Binding]
internal class BackoffStrategyConverter
{
    /// <summary>
    ///     Converts a string to a BackoffStrategy.
    /// </summary>
    /// <param name="value">
    ///     The string to convert. This should be one of the BackoffStrategy enum values, ignoring case.
    /// </param>
    /// <returns>
    ///     A BackoffStrategy value.
    /// </returns>
    [StepArgumentTransformation(@"(linear|exponential|fixed|fibonacci|polynomial|invalid)")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public BackoffStrategy Convert(string value)
    {
        if (value.Equals("invalid", StringComparison.OrdinalIgnoreCase))
        {
            return (BackoffStrategy)Int32.MaxValue;
        }
        return Enum.Parse<BackoffStrategy>(value, true);
    }
}