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

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (true)
        {
            try
            {
                return await taskToExecute().WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var nextAction = DetermineNextAction(ex, retryCount);
                switch (nextAction)
                {
                    case RetryAction.ThrowException: throw;
                    case RetryAction.Retry:
                        retryCount++;
                        // Try to sleep for the calculated duration,
                        await Task.Delay(DetermineDelayUntilNextIteration(retryCount), cancellationToken)
                            .ConfigureAwait(false);
                        break;
                    case RetryAction.GracefulExit:
                        return default!;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"{nextAction} is a unknown {nameof(RetryAction)} value.");
                }
            }
        }
    }

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="actionToExecute">
    ///     The action to execute.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token used to cancel the work.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the <see cref="RetryAction" /> is not supported.
    /// </exception>
    public async Task ExecuteAsync(Action actionToExecute, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (true)
        {
            try
            {
                actionToExecute();
                break;
            }
            catch (Exception ex)
            {
                var nextAction = DetermineNextAction(ex, retryCount);
                switch (nextAction)
                {
                    case RetryAction.ThrowException: throw;
                    case RetryAction.Retry:
                        retryCount++;
                        // Try to sleep for the calculated duration,
                        await Task.Delay(DetermineDelayUntilNextIteration(retryCount), cancellationToken)
                            .ConfigureAwait(false);
                        break;
                    case RetryAction.GracefulExit:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"{nextAction} is a unknown {nameof(RetryAction)} value.");
                }
            }
        }
    }

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="actionToExecute">
    ///     The action to execute.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token used to cancel the work.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the result.
    /// </typeparam>
    /// <returns>
    ///    The result of the operation.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///    Thrown when the <see cref="RetryAction" /> is not supported.
    /// </exception>
    public async Task<T> ExecuteAsync<T>(Func<T> actionToExecute, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (true)
        {
            try
            {
                var result = actionToExecute();
                return result;
            }
            catch (Exception ex)
            {
                var nextAction = DetermineNextAction(ex, retryCount);
                switch (nextAction)
                {
                    case RetryAction.ThrowException: throw;
                    case RetryAction.Retry:
                        retryCount++;
                        // Try to sleep for the calculated duration,
                        await Task.Delay(DetermineDelayUntilNextIteration(retryCount), cancellationToken)
                            .ConfigureAwait(false);
                        break;
                    case RetryAction.GracefulExit:
                        return default!;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"{nextAction} is a unknown {nameof(RetryAction)} value.");
                }
            }
        }
    }

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public async Task ExecuteAsync(Func<Task> taskToExecute, CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (true)
            try
            {
                await taskToExecute().WaitAsync(cancellationToken).ConfigureAwait(false);
                break;
            }
            catch (Exception ex)
            {
                var nextAction = DetermineNextAction(ex, retryCount);
                switch (nextAction)
                {
                    case RetryAction.ThrowException: throw;
                    case RetryAction.Retry:
                        retryCount++;
                        // Try to sleep for the calculated duration,
                        await Task.Delay(DetermineDelayUntilNextIteration(retryCount), cancellationToken)
                            .ConfigureAwait(false);
                        break;
                    case RetryAction.GracefulExit:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"{nextAction} is a unknown {nameof(RetryAction)} value.");
                }
            }
    }

    /// <summary>
    ///     Determines the next action to take based on the exception and the retry count.
    /// </summary>
    /// <param name="ex">
    ///     The exception that was thrown during the execution.
    /// </param>
    /// <param name="retryCount">
    ///    The current retry count.
    /// </param>
    /// <returns>
    ///     The next action to take.
    /// </returns>
    private RetryAction DetermineNextAction(Exception ex, int retryCount)
    {
        // throw if there are no corresponding callbacks (or) reached max retries.
        if (!_retryPolicy.ExceptionCallbacksByType.TryGetValue(ex.GetType(), out var exceptionCallback) ||
            (_retryPolicy.MaxRetries.HasValue && retryCount >= _retryPolicy.MaxRetries))
            return _retryPolicy.GracefulExceptionHandling ? RetryAction.GracefulExit : RetryAction.ThrowException;

        // the return bool value indicates whether to continue with retries or not.
        var continueExecution = exceptionCallback(ex);
        return continueExecution ? RetryAction.Retry : RetryAction.ThrowException;
    }

    /// <summary>
    ///     Determines the delay until the next iteration.
    /// </summary>
    /// <param name="retryCount">The current retry count.</param>
    /// <returns>The timespan to wait until the next iteration.</returns>
    /// <exception cref="NotImplementedException">Thrown when the specified backoff strategy is not yet supported</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified backoff strategy is out of range</exception>
    private TimeSpan DetermineDelayUntilNextIteration(int retryCount)
    {
        var delay = _retryPolicy.BackoffStrategy switch
        {
            BackoffStrategy.Linear => _retryPolicy.DelayBetweenRetries * retryCount,
            BackoffStrategy.Fixed => _retryPolicy.DelayBetweenRetries,
            BackoffStrategy.Exponential => _retryPolicy.DelayBetweenRetries * (retryCount * retryCount),
            BackoffStrategy.Fibonacci => _retryPolicy.DelayBetweenRetries * Fibonacci.CalculateNumberAtIndex(retryCount),
            BackoffStrategy.Polynomial => _retryPolicy.DelayBetweenRetries * Math.Pow(retryCount, _retryPolicy.PolynomialFactor),
            _ => throw new ArgumentOutOfRangeException($"{_retryPolicy.BackoffStrategy} is not a valid backoff strategy.")
        };
        return delay;
    }
}