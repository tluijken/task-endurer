using System.Diagnostics.CodeAnalysis;

namespace TaskEndurer;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum DelayStrategy
{
    /// <summary>
    ///     Increases the time between retries linearly.
    /// </summary>
    Linear,

    /// <summary>
    ///     Increases the time between retries exponentially by the retry count.
    /// </summary>
    Exponential
}
