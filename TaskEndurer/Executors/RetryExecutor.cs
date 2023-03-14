using System.Diagnostics.CodeAnalysis;
using TaskEndurer.Helpers;

namespace TaskEndurer.Executors;

/// <summary>
///     Basic <see cref="IRetryExecutor" /> implementation for retrying operations.
/// </summary>
[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
internal sealed class RetryExecutor : IRetryExecutor
{
    private readonly RetryPolicy _retryPolicy;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RetryExecutor" /> class.
    /// </summary>
    /// <param name="retryPolicy">
    ///     The retry configuration.
    /// </param>
    public RetryExecutor(RetryPolicy retryPolicy) => _retryPolicy = retryPolicy;

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public Task<T> ExecuteAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken = default) =>
        ExecuteAndCatchAsync(taskToExecute, cancellationToken);

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
    public Task ExecuteAsync(Action actionToExecute, CancellationToken cancellationToken = default) =>
        ExecuteAndCatchAsync(() =>
        {
            actionToExecute();
            return Task.FromResult(0);
        }, cancellationToken);

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
    ///     The result of the operation.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the <see cref="RetryAction" /> is not supported.
    /// </exception>
    public Task<T> ExecuteAsync<T>(Func<T> actionToExecute, CancellationToken cancellationToken = default) => 
        ExecuteAndCatchAsync(() => Task.Run(actionToExecute, cancellationToken), cancellationToken);

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public Task ExecuteAsync(Func<Task> taskToExecute, CancellationToken cancellationToken = default) =>
        ExecuteAndCatchAsync(async () =>
        {
            await taskToExecute().ConfigureAwait(false);
            return 0;
        }, cancellationToken);

    /// <summary>
    ///     Executes the specified task and catches any exceptions that are thrown.
    /// </summary>
    /// <param name="taskToExecute">
    ///    The task to execute.
    /// </param>
    /// <param name="cancellationToken">
    ///    A cancellation token used to cancel the work.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the result.
    /// </typeparam>
    /// <returns></returns>
    private async Task<T> ExecuteAndCatchAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken)
    {
        uint retryCount = 0;
        while (true)
        {
            try
            {
                var result = await taskToExecute().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                var nextAction = GetNextRetryAction(ex, retryCount);
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
                }
            }
        }
    }

    /// <summary>
    ///     The callback to invoke when an exception is thrown and the retry policy allows for a retry.
    /// </summary>
    private static readonly Func<Action<Exception>, Exception, RetryAction> HandleCallBackAndRetry = (callback, ex) =>
    {
        callback.Invoke(ex);
        return RetryAction.Retry;
    };

    /// <summary>
    ///     Determines the next action to take based on the exception and the retry count.
    /// </summary>
    /// <param name="ex">
    ///     The exception that was thrown during the execution.
    /// </param>
    /// <param name="retryCount">
    ///     The current retry count.
    /// </param>
    /// <returns>
    ///     The next action to take.
    /// </returns>
    private RetryAction GetNextRetryAction(Exception ex, uint retryCount)
    {
        var maxRetriesReached = _retryPolicy.MaxRetries.HasValue && retryCount >= _retryPolicy.MaxRetries;
        var exceptionCallback = _retryPolicy.ExceptionCallbacksByType.TryGetValue(ex.GetType(), out var cb) ? cb : null;
        return (!maxRetriesReached && exceptionCallback is not null) switch
        {
            // If the exception is in the list of exceptions to retry on and we have not reached the maximum number of retries,
            true => HandleCallBackAndRetry(exceptionCallback!, ex),
            // If the exception is not in the list of exceptions to retry on,
            // or if we have reached the maximum number of retries, we should throw the exception.
            // we should throw the exception.
            false => _retryPolicy.GracefulExceptionHandling ? RetryAction.GracefulExit : RetryAction.ThrowException
        };
    }

    /// <summary>
    ///     Determines the delay until the next iteration.
    /// </summary>
    /// <param name="retryCount">The current retry count.</param>
    /// <returns>The timespan to wait until the next iteration.</returns>
    /// <exception cref="NotImplementedException">Thrown when the specified backoff strategy is not yet supported</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified backoff strategy is out of range</exception>
    private TimeSpan DetermineDelayUntilNextIteration(uint retryCount) =>
        _retryPolicy.BackoffStrategy switch
        {
            BackoffStrategy.Linear => _retryPolicy.DelayBetweenRetries * retryCount,
            BackoffStrategy.Fixed => _retryPolicy.DelayBetweenRetries,
            BackoffStrategy.Exponential => _retryPolicy.DelayBetweenRetries * (retryCount * retryCount),
            BackoffStrategy.Fibonacci => _retryPolicy.DelayBetweenRetries * Fibonacci.CalculateNumberAtIndex(retryCount),
            BackoffStrategy.Polynomial => _retryPolicy.DelayBetweenRetries * Math.Pow(retryCount, _retryPolicy.PolynomialFactor),
            _ => throw new ArgumentOutOfRangeException(nameof(_retryPolicy.BackoffStrategy), _retryPolicy.BackoffStrategy, "is not a valid backoff strategy.")
        };
}