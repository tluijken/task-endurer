using TaskEndurer.Executors;

namespace TaskEndurer.Helpers;

internal static class RetryExecutorFactory
{
    // Construct the base executor.
    private static readonly Func<RetryPolicy, RetryExecutor> GetBaseExecutor = policy => new RetryExecutor(policy);

    // Use the until expired executor and decorate it with the base executor.
    private static readonly Func<RetryPolicy, UntilExpiredRetryExecutor> GetUntilExpiredExecutor = policy => new UntilExpiredRetryExecutor(policy, GetBaseExecutor(policy));

    /// <summary>
    ///     Default <see cref="IRetryExecutor" /> factory method.
    /// </summary>
    /// <param name="policy">
    ///     The retry policy to use.
    /// </param>
    /// <returns>
    ///     A retry executor for executing tasks and retry as long as the policy permits this.
    /// </returns>
    internal static IRetryExecutor Create(RetryPolicy policy) => policy.MaxDuration.HasValue ? GetUntilExpiredExecutor(policy) : GetBaseExecutor(policy);
}