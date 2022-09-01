namespace TaskEndurer;

public enum RetryAction
{
    ThrowException,
    Retry,
    GracefulExit
}