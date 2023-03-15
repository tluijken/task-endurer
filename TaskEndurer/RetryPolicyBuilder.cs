using TaskEndurer.Helpers;

namespace TaskEndurer;

public class RetryPolicyBuilder : GenericFunctionalBuilder<RetryPolicy, RetryPolicyBuilder>
{
    /// <summary>
    ///     Creates a new instance of the <see cref="RetryPolicyBuilder" /> class.
    /// </summary>
    /// <returns>
    ///    An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public static RetryPolicyBuilder Create() => new();

    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithGracefulExceptionHandling() => Do(policy => policy.GracefulExceptionHandling = true);

    /// <summary>
    ///     Specified the delay used between retries.
    /// </summary>
    /// <remarks>
    ///     Default is 100 milliseconds.
    /// </remarks>
    /// <param name="delayBetweenRetries">The delay to use between retries.</param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithDelay(TimeSpan delayBetweenRetries) => Do(policy => policy.DelayBetweenRetries = delayBetweenRetries);

    /// <summary>
    ///     Specified the maximum number of retries.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithMaxRetries(uint maxRetries) => Do(policy => policy.MaxRetries = maxRetries);

    /// <summary>
    ///     Specified the delay strategy.
    /// </summary>
    /// <example>
    ///     <see cref="BackoffStrategy.Linear" /> will keep the same delay between retries.
    ///     <see cref="BackoffStrategy.Exponential" /> will exponentially increase the delay between retries.
    /// </example>
    /// <param name="backoffStrategy">
    ///     The delay strategy to use.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithBackoff(BackoffStrategy backoffStrategy) => Do(policy => policy.BackoffStrategy = backoffStrategy);

    /// <summary>
    ///     Specified the delay strategy.
    /// </summary>
    /// <param name="polynomialBackoffStrategy">
    ///     The polynomial backoff strategy containing the exponential factor to use.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///    Thrown if <paramref name="polynomialBackoffStrategy" /> is null.
    /// </exception>
    public RetryPolicyBuilder WithBackoff(PolynomialBackoffStrategy polynomialBackoffStrategy)
    {
        ArgumentNullException.ThrowIfNull(polynomialBackoffStrategy);
        Do(policy => policy.BackoffStrategy = BackoffStrategy.Polynomial);
        return WithPolynomialFactor(polynomialBackoffStrategy.PolynomialFactor);
    }

    /// <summary>
    ///     The maximum amount of time to allow retries to occur.
    /// </summary>
    /// <param name="maxDuration">
    ///     The maximum amount of time to allow retries to occur.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithMaxDuration(TimeSpan maxDuration) => Do(policy => policy.MaxDuration = maxDuration);

    /// <summary>
    ///     Builds the retry policy and returns an executor that can be used to execute the action.
    /// </summary>
    /// <returns>an <see cref="IRetryExecutor" /> that can be used to execute the action</returns>
    public new IRetryExecutor Build() => RetryExecutorFactory.Create(base.Build());

    /// <summary>
    ///     Specifies if the retry policy should retry whenever the <typeparamref name="TException" /> occurs.
    /// </summary>
    /// <remarks>
    ///     This means that unexpected exceptions will not be caught and will be thrown.
    /// </remarks>
    /// <typeparam name="TException">
    ///     The exception type to retry on.
    /// </typeparam>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithExpectedException<TException>() where TException : Exception => Do(policy => policy.RegisterExceptionCallback<TException>(_ => { }));

    /// <summary>
    ///     Registers a callback that will be invoked whenever the <typeparamref name="TException" /> occurs.
    /// </summary>
    /// <remarks>
    ///     Typically useful for additional logging or other side effects.
    /// </remarks>
    /// <param name="exceptionCallback">
    ///     The callback to invoke.
    /// </param>
    /// <typeparam name="TException">
    ///     The exception type to register the callback for.
    /// </typeparam>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    public RetryPolicyBuilder WithExceptionHandling<TException>(Action<TException> exceptionCallback) where TException : Exception => Do(policy => policy.RegisterExceptionCallback<TException>(ex => exceptionCallback((TException)ex)));

    /// <summary>
    ///     Specifies the factor to use when calculating the delay between retries when using the <see cref="BackoffStrategy.Polynomial" /> strategy.
    /// </summary>
    /// <param name="factor">
    ///     The polynomial factor to use.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="RetryPolicyBuilder" />.
    /// </returns>
    private RetryPolicyBuilder WithPolynomialFactor(double factor) =>
        factor switch
        {
            <= 0 => throw new ArgumentOutOfRangeException(nameof(factor),
                "The polynomial factor must be greater than zero."),
            _ => Do(policy => policy.PolynomialFactor = factor)
        };
}