namespace TaskEndurer.Helpers;

public record RetryPolicy
{
    public RetryPolicy()
    {
        MaxRetries = null;
        DelayBetweenRetries = TimeSpan.FromMilliseconds(100);
        MaxDuration = null;
        GracefulExceptionHandling = false;
        PolynomialFactor = 2;
    }

    /// <summary>
    ///     The factor to use for the polynomial backoff strategy.
    /// </summary>
    internal double PolynomialFactor { get; set; }

    /// <summary>
    ///     Registers an exception callback, which is called whenever an exception of type <typeparamref name="TException" />
    ///     occurs.
    /// </summary>
    /// <param name="callback">The callback to execute whenever an exception of type <typeparamref name="TException" /> occurs.</param>
    /// <typeparam name="TException">The type of exception to handle.</typeparam>
    internal void RegisterExceptionCallback<TException>(Action<Exception> callback) where TException : Exception
    {
        ExceptionCallbacksByType.Add(typeof(TException), callback);
    }

    /// <summary>
    ///     Registered exception types that should be retried.
    /// </summary>
    internal Dictionary<Type, Action<Exception>> ExceptionCallbacksByType { get; } = new();

    /// <summary>
    ///     The maximum number of retries.
    /// </summary>
    internal uint? MaxRetries { get; set; }

    /// <summary>
    ///     The delay in seconds between retries.
    /// </summary>
    internal TimeSpan DelayBetweenRetries { get; set; }

    /// <summary>
    ///     The delay in seconds between retries.
    /// </summary>
    internal TimeSpan? MaxDuration { get; set; }

    /// <summary>
    ///     Defines the delay strategy to use between retries.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="TaskEndurer.BackoffStrategy.Fixed" />.
    /// </remarks>
    internal BackoffStrategy BackoffStrategy { get; set; } = BackoffStrategy.Fixed;

    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    internal bool GracefulExceptionHandling { get; set; }
}