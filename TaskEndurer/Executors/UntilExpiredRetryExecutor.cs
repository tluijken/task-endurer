using TaskEndurer.Helpers;

namespace TaskEndurer.Executors;

/// <summary>
///     Basic <see cref="IRetryExecutor" /> implementation for retrying operations.
/// </summary>
internal sealed class UntilExpiredRetryExecutor : IRetryExecutor
{
    private readonly IRetryExecutor _decorated;
    private readonly RetryPolicy _retryPolicy;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UntilExpiredRetryExecutor" /> class.
    /// </summary>
    /// <param name="retryPolicy">
    ///     The retry configuration.
    /// </param>
    /// <param name="decorated">A decorated retry executor to </param>
    public UntilExpiredRetryExecutor(RetryPolicy retryPolicy, IRetryExecutor decorated)
    {
        _retryPolicy = retryPolicy;
        _decorated = decorated;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken = default)
    {
        if (!_retryPolicy.MaxDuration.HasValue)
            throw new NotSupportedException(
                "The maximum duration is not set and cannot be used with an executor that waits until the timespan is expired.");

        using var maximumWaitCancellationToken = new CancellationTokenSource(_retryPolicy.MaxDuration.Value);
        using var cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, maximumWaitCancellationToken.Token);

        // Execute the task.
        return await _decorated.ExecuteAsync(taskToExecute, cancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task ExecuteAsync(Func<Action> actionToExecute, CancellationToken cancellationToken = default)
    {
        if (!_retryPolicy.MaxDuration.HasValue)
        {
            throw new NotSupportedException(
                "The maximum duration is not set and cannot be used with an executor that waits until the timespan is expired.");
        }

        using var maximumWaitCancellationToken = new CancellationTokenSource(_retryPolicy.MaxDuration.Value);
        using var cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, maximumWaitCancellationToken.Token);

        // Execute the task.
        await _decorated.ExecuteAsync(actionToExecute, cancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task<T> ExecuteAsync<T>(Func<T> actionToExecute, CancellationToken cancellationToken = default)
    {
        if (!_retryPolicy.MaxDuration.HasValue)
        {
            throw new NotSupportedException(
                "The maximum duration is not set and cannot be used with an executor that waits until the timespan is expired.");
        }

        using var maximumWaitCancellationToken = new CancellationTokenSource(_retryPolicy.MaxDuration.Value);
        using var cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, maximumWaitCancellationToken.Token);

        // Execute the task.
        return await _decorated.ExecuteAsync(actionToExecute, cancellationTokenSource.Token).ConfigureAwait(false);
    }


    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    public async Task ExecuteAsync(Func<Task> taskToExecute, CancellationToken cancellationToken = default)
    {
        if (!_retryPolicy.MaxDuration.HasValue)
        {
            throw new NotSupportedException(
                "The maximum duration is not set and cannot be used with an executor that waits until the timespan is expired.");
        }

        using var maximumWaitCancellationToken = new CancellationTokenSource(_retryPolicy.MaxDuration.Value);
        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, maximumWaitCancellationToken.Token);
        // Execute the task.
        await _decorated.ExecuteAsync(taskToExecute, cancellationTokenSource.Token).ConfigureAwait(false);
    }
}