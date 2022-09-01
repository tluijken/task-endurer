using TaskEndurer.Helpers;

namespace TaskEndurer;

public sealed class RetryPolicyBuilder : IRetryPolicyBuilder
{
    private RetryPolicy _retryPolicy;

    /// <summary>
    ///     Creates a new instance of the <see cref="RetryPolicyBuilder"/> class.
    /// </summary>
    /// <returns></returns>
    public static IRetryPolicyBuilder Create()
    {
        return new RetryPolicyBuilder();
    }


    /// <summary>
    ///     Creates a new instance of the <see cref="RetryPolicyBuilder"/> class.
    /// </summary>
    private RetryPolicyBuilder()
    {
        _retryPolicy = new RetryPolicy();
    }


    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    /// <returns></returns>
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
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    public IRetryPolicyBuilder WithDelayBetweenRetries(TimeSpan delayBetweenRetries)
    {
        _retryPolicy.DelayBetweenRetries = delayBetweenRetries;
        return this;
    }

    /// <summary>
    ///     Specified the maximum number of retries.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    public IRetryPolicyBuilder WithMaxRetries(int maxRetries)
    {
        _retryPolicy.MaxRetries = maxRetries;
        return this;
    }

    public IRetryPolicyBuilder WithDelayStrategy(DelayStrategy delayStrategy)
    {
        _retryPolicy.DelayStrategy = delayStrategy;
        return this;
    }

    public IRetryPolicyBuilder WithMaxDuration(TimeSpan maxDuration)
    {
        _retryPolicy.MaxDuration = maxDuration;
        return this;
    }

    public IRetryPolicyBuilder ContinueOnException<TException>(bool retryOnException) where TException : Exception
    {
        _retryPolicy.RegisterExceptionCallback<TException>(() => retryOnException);
        return this;
    }

    public IRetryExecutor Build()
    {
        return RetryExecutorFactory.Create(_retryPolicy);
    }
}

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
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    IRetryPolicyBuilder WithDelayBetweenRetries(TimeSpan delayBetweenRetries);

    /// <summary>
    ///     Specified the maximum number of retries.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    IRetryPolicyBuilder WithMaxRetries(int maxRetries);

    /// <summary>
    ///     Specified the delay strategy.
    /// </summary>
    /// <example>
    ///     <see cref="DelayStrategy.Linear"/> will keep the same delay between retries.
    ///     <see cref="DelayStrategy.Exponential"/> will exponentially increase the delay between retries.
    /// </example>
    /// <param name="delayStrategy">
    ///     The delay strategy to use.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    IRetryPolicyBuilder WithDelayStrategy(DelayStrategy delayStrategy);

    /// <summary>
    ///     The maximum amount of time to allow retries to occur.
    /// </summary>
    /// <param name="maxDuration">
    ///    The maximum amount of time to allow retries to occur.
    /// </param>
    /// <returns>
    ///     An instance of <see cref="IRetryPolicyBuilder"/>.
    /// </returns>
    IRetryPolicyBuilder WithMaxDuration(TimeSpan maxDuration);

    /// <summary>
    ///     Specifies if the retry policy should retry whenever the <typeparamref name="TException"/> occurs.
    /// </summary>
    /// <param name="retryOnException">
    ///     Indicates whether the retry policy should continue retrying if this type of exception occurs.
    /// </param>
    /// <typeparam name="TException">The exception type to retry on.</typeparam>
    /// <returns></returns>
    IRetryPolicyBuilder ContinueOnException<TException>(bool retryOnException) where TException : Exception;

    /// <summary>
    ///     Builds the retry policy and returns an executor that can be used to execute the action.
    /// </summary>
    /// <returns>an <see cref="IRetryExecutor"/> that can be used to execute the action</returns>
    IRetryExecutor Build();
}
