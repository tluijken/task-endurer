namespace TaskEndurer;

/// <summary>
///     Often, when making function calls, something goes wrong. The internet might glitch. The API we’re calling might
///     timeout. Gremlins might eat your packets. Any number of things can go wrong, and Murphy’s law tells us that they
///     will.
///     Which is why we need backoff strategies. Basically, a backoff strategy is a technique that we can use to retry
///     failing function calls after a given delay - and keep retrying them until either the function call works, or until
///     we’ve tried so many times that we just give up and handle the error.
///     <a href="https://backoff-utils.readthedocs.io">See more detailed information about backoff strategies here</a>
/// </summary>
public enum BackoffStrategy
{
    /// <summary>
    ///     The base delay time is equal to the attempt count.
    /// </summary>
    Linear,

    /// <summary>
    ///     Increases the time between retries exponentially by the retry count.
    /// </summary>
    Exponential,

    /// <summary>
    ///     The base delay time is returned as the Fibonacci number corresponding to the current attempt.
    /// </summary>
    Fibonacci,

    /// <summary>
    ///     The base delay time is calculated as a fixed value.
    /// </summary>
    Fixed,

    /// <summary>
    ///     The base delay time is calculated as: 'ae' where:a is the number of unsuccessful attempts that have been made, e is
    ///     the exponent configured for the strategy.
    /// </summary>
    Polynomial
}