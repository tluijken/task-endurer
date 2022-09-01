using TaskEndurer.Helpers;

namespace TaskEndurer.Executors;

/// <summary>
///     Basic <see cref="IRetryExecutor" /> implementation for retrying operations.
/// </summary>
internal sealed class RetryExecutor : IRetryExecutor
{
    private readonly RetryPolicy _retryPolicy;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RetryExecutor" /> class.
    /// </summary>
    /// <param name="retryPolicy">
    ///     The retry configuration.
    /// </param>
    public RetryExecutor(RetryPolicy retryPolicy)
    {
        _retryPolicy = retryPolicy;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        if (taskToExecute is null)
        {
            throw new InvalidOperationException("Cannot execute a task that has not been set.");
        }

        while (true)
        {
            try
            {
                return await taskToExecute().WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // throw if there are no corresponding callbacks (or) reached max retries.
                if (!_retryPolicy.ExceptionCallbacksByType.TryGetValue(ex.GetType(), out var exceptionCallback) ||
                    (_retryPolicy.MaxRetries.HasValue && retryCount >= _retryPolicy.MaxRetries))
                {
                    if (_retryPolicy.GracefulExceptionHandling)
                    {
                        return default!;
                    }
                    throw;
                }

                // the return bool value indicates whether to continue with retries or not.
                var continueExecution = exceptionCallback();
                if (!continueExecution)
                {
                    throw;
                }

                retryCount++;
                var delay = _retryPolicy.DelayStrategy is DelayStrategy.Linear
                    // linear back-off i.e. the duration to wait between retries set delay between retries.
                    ? _retryPolicy.DelayBetweenRetries
                    // exponential back-off i.e. the duration to wait between retries based on the current retry attempt.
                    : _retryPolicy.DelayBetweenRetries * retryCount;

                // Try to sleep for the specified duration, if the cancellation token gets cancelled during the wait, we don't want to retry and return immediately.
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute"></param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public async Task ExecuteAsync(Func<Task> taskToExecute, CancellationToken cancellationToken = default)
    {
        if (taskToExecute is null)
        {
            throw new InvalidOperationException("Cannot execute a task that has not been set.");
        }

        var retryCount = 0;
        while (true)
        {
            try
            {
                await taskToExecute().WaitAsync(cancellationToken).ConfigureAwait(false);
                break;
            }
            catch (Exception ex)
            {
                // throw if there are no corresponding callbacks (or) reached max retries.
                if (!_retryPolicy.ExceptionCallbacksByType.TryGetValue(ex.GetType(), out var exceptionCallback) ||
                    (_retryPolicy.MaxRetries.HasValue && retryCount >= _retryPolicy.MaxRetries))
                {
                    if (_retryPolicy.GracefulExceptionHandling)
                    {
                        return;
                    }
                    throw;
                }

                // the return bool value indicates whether to continue with retries or not.
                var continueExecution = exceptionCallback();
                if (!continueExecution)
                {
                    throw;
                }

                retryCount++;
                var delay = _retryPolicy.DelayStrategy is DelayStrategy.Linear
                    // linear back-off i.e. the duration to wait between retries set delay between retries.
                    ? _retryPolicy.DelayBetweenRetries
                    // exponential back-off i.e. the duration to wait between retries based on the current retry attempt.
                    : _retryPolicy.DelayBetweenRetries * retryCount;

                // Try to sleep for the specified duration, if the cancellation token gets cancelled during the wait, we don't want to retry and return immediately.
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
