namespace TaskEndurer;

public interface IRetryExecutor
{
    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    Task ExecuteAsync(Func<Task> taskToExecute, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="taskToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    Task<T> ExecuteAsync<T>(Func<Task<T>> taskToExecute, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="actionToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    Task ExecuteAsync(Func<Action> actionToExecute, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Retries the specified operation, as long as the retry policy allows it.
    /// </summary>
    /// <param name="actionToExecute">The action to execute.</param>
    /// <param name="cancellationToken">A cancellation token used to cancel the work.</param>
    /// <returns>An awaitable task.</returns>
    Task<T> ExecuteAsync<T>(Func<T> actionToExecute, CancellationToken cancellationToken = default);
}