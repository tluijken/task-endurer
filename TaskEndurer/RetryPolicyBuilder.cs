using TaskEndurer.Helpers;

namespace TaskEndurer;

/// <summary>
///     A class that can be used to build a <see cref="RetryPolicy" />.
/// </summary>
public sealed class RetryPolicyBuilder : IRetryPolicyBuilder
{
    private RetryPolicy _retryPolicy;

    /// <summary>
    ///     Creates a new instance of the <see cref="RetryPolicyBuilder" /> class.
    /// </summary>
    private RetryPolicyBuilder()
    {
        _retryPolicy = new RetryPolicy();
    }


    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithGracefulExceptionHandling()
    {
        _retryPolicy.GracefulExceptionHandling = true;
        return this;
    }

    /// <summary>
    ///     Specified the delay used between retries.
    /// </summary>
    /// <remarks>
    ///     Default is 100 milliseconds.
    /// </remarks>
    /// <param name="delayBetweenRetries">The delay to use between retries.</param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithDelay(TimeSpan delayBetweenRetries)
    {
        _retryPolicy.DelayBetweenRetries = delayBetweenRetries;
        return this;
    }

    /// <summary>
    ///     Specified the maximum number of retries.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithMaxRetries(int maxRetries)
    {
        _retryPolicy.MaxRetries = maxRetries;
        return this;
    }

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
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithBackoff(BackoffStrategy backoffStrategy)
    {
        _retryPolicy.BackoffStrategy = backoffStrategy;
        return this;
    }

    /// <summary>
    ///     The maximum amount of time to allow retries to occur.
    /// </summary>
    /// <param name="maxDuration">
    ///     The maximum amount of time to allow retries to occur.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithMaxDuration(TimeSpan maxDuration)
    {
        _retryPolicy.MaxDuration = maxDuration;
        return this;
    }

    /// <summary>
    ///     Specifies if the retry policy should retry whenever the <typeparamref name="TException" /> occurs.
    /// </summary>
    /// <param name="retryOnException">
    ///     Indicates whether the retry policy should continue retrying if this type of exception occurs.
    /// </param>
    /// <typeparam name="TException">The exception type to retry on.</typeparam>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    [Obsolete("Use WithExpectedException instead.")]
    public IRetryPolicyBuilder ContinueOnException<TException>(bool retryOnException) where TException : Exception
    {
        _retryPolicy.RegisterExceptionCallback<TException>(_ => retryOnException);
        return this;
    }

    /// <summary>
    ///     Builds the retry policy and returns an executor that can be used to execute the action.
    /// </summary>
    /// <returns>an <see cref="IRetryExecutor" /> that can be used to execute the action</returns>
    public IRetryExecutor Build()
    {
        return RetryExecutorFactory.Create(_retryPolicy);
    }

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
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithExpectedException<TException>() where TException : Exception
    {
        _retryPolicy.RegisterExceptionCallback<TException>(_ => true);
        return this;
    }

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
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    public IRetryPolicyBuilder WithExceptionHandling<TException>(Action<TException> exceptionCallback)
        where TException : Exception
    {
        _retryPolicy.RegisterExceptionCallback<TException>(ex =>
        {
            exceptionCallback.Invoke((TException)ex);
            return true;
        });
        return this;
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="RetryPolicyBuilder" /> class.
    /// </summary>
    /// <returns></returns>
    public static IRetryPolicyBuilder Create()
    {
        return new RetryPolicyBuilder();
    }
}