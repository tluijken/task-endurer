using System;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace TaskEndurer.Tests.Converters;

[Binding]
internal class BooleanConverter
{
    /// <summary>
    ///     Converts a string to a boolean.
    /// </summary>
    /// <param name="value">
    ///     The string to convert. This should be either "true" or "false", ignoring case.
    /// </param>
    /// <returns>
    ///     A boolean value.
    /// </returns>
    [StepArgumentTransformation(@"(true|false)")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool Convert(string value)
    {
        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }
}