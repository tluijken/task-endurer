using System;

namespace TaskEndurer.Tests;

internal static class Constants
{
    internal const string MaxFailCountKey = "maxRetryCount";
    internal const string RetryCountKey = "retryCount";
    internal const string RetryExecutorKey = "retryExecutor";
    internal const string RetryPolicyBuilderKey = "retryExecutorBuilder";
    internal const string RetryExceptionKey = "retryException";
    internal const string TaskResultKey = "taskResult";
    internal const string RetryPolicyKey = "retryPolicy";
    internal const string IncrementalResultKey = "incrementalResult";
    internal static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromMilliseconds(100);
    public const string StartTimeKey = "startTime";
    internal const string RetryResultKey = "retryResult";
}