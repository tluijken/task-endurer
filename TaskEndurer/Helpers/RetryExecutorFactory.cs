using TaskEndurer.Executors;

namespace TaskEndurer.Helpers;

internal static class RetryExecutorFactory
{
    /// <summary>
    ///     Default <see cref="IRetryExecutor" /> factory method.
    /// </summary>
    /// <param name="policy">
    ///     The retry policy to use.
    /// </param>
    /// <returns>
    ///     A retry executor for executing tasks and retry as long as the policy permits this.
    /// </returns>
    internal static IRetryExecutor Create(RetryPolicy policy = default)
    {
        // We will always use the default retry policy.
        var baseExecutor = new RetryExecutor(policy);
       
        return policy.MaxDuration.HasValue ?
            // Use the until expired executor and decorate it with the base executor.
            new UntilExpiredRetryExecutor(policy, baseExecutor) :
            // Otherwise the base executor.
            baseExecutor;
    }
}
