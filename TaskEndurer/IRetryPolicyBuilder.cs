namespace TaskEndurer;

public interface IRetryPolicyBuilder
{
    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    /// <returns></returns>
    IRetryPolicyBuilder WithGracefulExceptionHandling();

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
    IRetryPolicyBuilder WithDelay(TimeSpan delayBetweenRetries);

    /// <summary>
    ///     Specified the maximum number of retries before the task will actually fail.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    IRetryPolicyBuilder WithMaxRetries(int maxRetries);

    /// <summary>
    ///     Specified the backoff strategy.
    /// </summary>
    /// <example>
    ///     <see cref="BackoffStrategy.Fixed" /> will keep the same delay between retries.
    ///     <see cref="BackoffStrategy.Exponential" /> will exponentially increase the delay between retries.
    /// </example>
    /// <param name="backoffStrategy">
    ///     The delay strategy to use.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    IRetryPolicyBuilder WithBackoff(BackoffStrategy backoffStrategy);

    /// <summary>
    ///     The maximum amount of time to allow retries to occur.
    /// </summary>
    /// <param name="maxDuration">
    ///     The maximum amount of time to allow retries to occur.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder" />.
    /// </returns>
    IRetryPolicyBuilder WithMaxDuration(TimeSpan maxDuration);

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
    IRetryPolicyBuilder ContinueOnException<TException>(bool retryOnException) where TException : Exception;

    /// <summary>
    ///     Builds the retry policy and returns an executor that can be used to execute the action.
    /// </summary>
    /// <returns>an <see cref="IRetryExecutor" /> that can be used to execute the action</returns>
    IRetryExecutor Build();
}