using System.Diagnostics.CodeAnalysis;

namespace TaskEndurer.Helpers;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "CA1815")]
internal struct RetryPolicy
{
    public RetryPolicy()
    {
        MaxRetries = null;
        DelayBetweenRetries = TimeSpan.FromMilliseconds(100);
        MaxDuration = null;
        GracefulExceptionHandling = false;
    }

    /// <summary>
    ///     Registers an exception callback, which is called whenever an exception of type <typeparamref name="TException" />
    ///     occurs.
    /// </summary>
    /// <param name="callback">The callback to execute whenever an exception of type <typeparamref name="TException" /> occurs.</param>
    /// <typeparam name="TException">The type of exception to handle.</typeparam>
    internal void RegisterExceptionCallback<TException>(Func<Exception, bool> callback) where TException : Exception
    {
        ExceptionCallbacksByType.Add(typeof(TException), callback);
    }

    /// <summary>
    ///     Registered exception types that should be retried.
    /// </summary>
    internal Dictionary<Type, Func<Exception, bool>> ExceptionCallbacksByType { get; } = new();

    /// <summary>
    ///     The maximum number of retries.
    /// </summary>
    public int? MaxRetries { get; internal set; }

    /// <summary>
    ///     The delay in seconds between retries.
    /// </summary>
    public TimeSpan DelayBetweenRetries { get; internal set; }

    /// <summary>
    ///     The delay in seconds between retries.
    /// </summary>
    public TimeSpan? MaxDuration { get; internal set; }

    /// <summary>
    ///     Defines the delay strategy to use between retries.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="TaskEndurer.BackoffStrategy.Fixed" />.
    /// </remarks>
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public BackoffStrategy BackoffStrategy { get; internal set; } = BackoffStrategy.Fixed;

    /// <summary>
    ///     Specifies that any exceptions should be gracefully handled and not thrown once the retry count has been reached.
    /// </summary>
    public bool GracefulExceptionHandling { get; internal set; }
}