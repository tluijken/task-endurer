[![Code coverage tests](https://github.com/tluijken/task-endurer/actions/workflows/CODE_COVERAGE_TESTS.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/CODE_COVERAGE_TESTS.yml)
[![Publish Packages](https://github.com/tluijken/task-endurer/actions/workflows/PUBLISH_PACKAGES.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/PUBLISH_PACKAGES.yml)
[![codecov](https://codecov.io/gh/tluijken/task-endurer/branch/main/graph/badge.svg)](https://app.codecov.io/gh/tluijken/task-endurer)
[![NuGet version (TaskEndurer)](https://img.shields.io/nuget/v/TaskEndurer.svg?style=flat-square)](https://www.nuget.org/packages/TaskEndurer/)

## Summary

Task Endurer is a library that lets you easily retry asynchronous tasks based on a given policy.

### Getting started

Installation via Package Manager Console in Visual Studio:

```powershell
PM> Install-Package TaskEndurer
```

Installation via .NET CLI:

```console
> dotnet add <TARGET PROJECT> package TaskEndurer
```

## Usage
The main goal of Task Endurer is to retry a task based on a given policy in a minimal fashion. 
The policy is defined by the `RetryPolicy` class, which is constructed by the `RetryPolicyBuilder` class.

### Example

```csharp
var policy = RetryPolicyBuilder().Create() // create a policy builder
    .WithMaxRetries(3) // Maximum number of retries is 3
    .WithDelay(TimeSpan.FromSeconds(1)) // Delay between retries is 1 second
```

The policy can then be used to build an executor:
The executor can be used to execute a task. The task will be retried if it fails, based on the policy.

```csharp
var executor = policy.Build(); // Build the executor based on the policy.
executor.ExecuteAsync(async () => await Task.Delay(1000)); // Execute the task using the executor.
```
## Policy builder options
| Option                        | Description                                                                                                                                          |
|-------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------|
| WithMaxRetries                | Specified the maximum number of retries before the task will actually fail.                                                                          |
| WithDelay                     | Specifies the delay between retries.                                                                                                                 |
| WithBackoff                   | Specifies the backoff strategy.                                                                                                                      |
| WithMaxDuration               | Specifies the maximum duration to retry.                                                                                                             |
| WithExpectedException         | Specifies which exception types to expect.                                                                                                           |
| WithGracefulExceptionHandling | Specifies that any exceptions should be gracefully handled and not thrown after the maximum duration or maximum number of retries have been reached. |
| WithExceptionCallback         | Registers a callback that will be called when an exception of a specified type occurs.                                                               |

## Backoff strategies
| Strategy    | Description                                                                                                                                                   |
|-------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Linear      | The base delay time is equal to the attempt count.                                                                                                            |
| Exponential | Increases the time between retries exponentially by the retry count.                                                                                          |
| Fibonacci   | The base delay time is returned as the Fibonacci number corresponding to the current attempt.                                                                 |
| Fixed       | The base delay time is calculated as a fixed value.                                                                                                           |
| Polynomial  | The base delay time is calculated as: ae where:a is the number of unsuccessful attempts that have been made, e is the exponent configured for the strategy.   |