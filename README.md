[![Code coverage tests](https://github.com/tluijken/task-endurer/actions/workflows/CODE_COVERAGE_TESTS.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/CODE_COVERAGE_TESTS.yml)
[![Publish Packages](https://github.com/tluijken/task-endurer/actions/workflows/PUBLISH_PACKAGES.yml/badge.svg)](https://github.com/tluijken/task-endurer/actions/workflows/PUBLISH_PACKAGES.yml)
[![codecov](https://codecov.io/gh/tluijken/task-endurer/branch/main/graph/badge.svg)](https://app.codecov.io/gh/tluijken/task-endurer)
[![NuGet version (TaskEndurer)](https://img.shields.io/nuget/v/TaskEndurer.svg?style=flat-square)](https://www.nuget.org/packages/TaskEndurer/)

# TaskEndurer
This opinionated library simplifies the process of retrying failed operations. With TaskEndurer, developers can easily define retry policies using an intuitive API, allowing for efficient execution of operations based on these policies.

While other fault handling libraries offer powerful features, their vast array of options can be overwhelming, especially for simple scenarios. TaskEndurer embraces the KISS principle, prioritizing simplicity and ease of use without sacrificing functionality.

## Don't catch them all.
By default, TaskEndurer does not catch any exceptions. Instead, developers must specify which exceptions to expect. This approach encourages developers to anticipate and handle potential exceptions upfront, allowing for more robust and resilient code.

Our goal is to ensure that only exceptions that the system can recover from are handled. In the event of an unexpected exception, developers will be alerted and prompted to take appropriate measures to address the unexpected behavior.

Additionally, this approach encourages developers to consider which types of exceptions to throw when implementing operations, thereby promoting code consistency and readability.


## Getting started

Installation via Package Manager Console in Visual Studio:

```powershell
PM> Install-Package TaskEndurer
```

Installation via .NET CLI:

```console
> dotnet add <TARGET PROJECT> package TaskEndurer
```

## Usage

### Constructing your policy.
Here is a full example, displaying all the current features of TaskEndurer.
```csharp
var policy = RetryPolicyBuilder.Create()
    // Specify the delay between retries.
    .WithDelay(TimeSpan.FromMilliseconds(100))
    // Specify the backoff strategy.
    .WithBackoff(BackoffStrategy.Exponential)
    // Specify the maximum number of retries.
    .WithMaxRetries(3)
    // Specify the maximum duration of the retry policy.
    .WithMaxDuration(TimeSpan.FromSeconds(60))
    // Specify which exception types to retry on.
    .WithExpectedException<OperationCanceledException>()
    // Add a custom exception handler. If you cannot recover from the exception, you can throw it again.
    .WithExceptionHandling<HttpRequestException>(ex => Console.WriteLine($"Unable to handle HttpRequest: {ex}"))
    // If the operation fails after all retries, the program can continue gracefully.
    .WithGracefulExceptionHandling()
    // Sets the exponential factor for the polynomial backoff strategy.
    .WithPolynomialFactor(2);
```

Let's brake it down.
To define your retry policy, you can use the RetryPolicyBuilder class.

Start by creating an instance of the RetryPolicyBuilder:
```csharp
var policy = RetryPolicyBuilder.Create()
```
Then, you can customize your retry policy using the available methods:

* `WithDelay` method: set the delay between retries. For example, to set a delay of 100 milliseconds between each retry, use the following code:
```csharp
.WithDelay(TimeSpan.FromMilliseconds(100))
```
*  `WithBackoff` method: choose a backoff strategy for the retry policy. For example, to use an exponential backoff strategy, use the following code:
```csharp
.WithBackoff(BackoffStrategy.Exponential)
```
* `WithMaxRetries` method: set the maximum number of retries for the operation. This setting is optional, and can be used together with the `WithMaxDuration`. Depending on whether the max duration, or the maximumm amount of retries exceeds first, the operation will be terminated. For example, to set a maximum of 3 retries, use the following code:
```csharp
.WithMaxRetries(3)
```

* `WithMaxDuration` method: set the maximum duration for the operation. For example, to set a maximum duration of 60 seconds, use the following code:
```csharp
.WithMaxDuration(TimeSpan.FromSeconds(60))
```

* `WithExpectedException` method: specify which exception types to retry on. You can use this method for multiple exception types. For example, to retry on OperationCanceledException, use the following code:
```csharp
.WithExpectedException<OperationCanceledException>()
```

* `WithExceptionHandling` method: add a custom exception handler. This method takes the type of the exception to handle and a delegate that specifies the action to take when the exception is thrown. For example, to handle HttpRequestException and print an error message to the console, use the following code:
```csharp
.WithExceptionHandling<HttpRequestException>(ex => Console.WriteLine($"Unable to handle HttpRequest: {ex}"))
```

* `WithGracefulExceptionHandling` method: if the operation fails after all retries, this method allows the program to continue gracefully.
```csharp
.WithGracefulExceptionHandling()
```

* `WithPolynomialFactor` method: sets the exponential factor for the polynomial backoff strategy. For example, to set the factor to 2, use the following code:
```csharp
.WithPolynomialFactor(2)
```

### Building and using the executor

After customizing your retry policy, you can execute the operation using the ExecuteAsync method of the policy. Here's an example of executing an HTTP request with the policy:

```csharp 
var executor = policy.Build();
var response = await policy.ExecuteAsync(async () => await httpClient.GetAsync("https://www.github.com"));
```

## Policy builder options
| Option                        | Description                                                                                                                                          | Defaults to |
|-------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------|-------------|
| WithMaxRetries                | Specified the maximum number of retries before the task will actually fail.                                                                          | null        |
| WithDelay                     | Specifies the delay between retries.                                                                                                                 | 100 Ms      |
| WithBackoff                   | Specifies the backoff strategy.                                                                                                                      | Fixed       |
| WithMaxDuration               | Specifies the maximum duration to retry.                                                                                                             | null        |
| WithExpectedException         | Specifies which exception types to expect.                                                                                                           | -           |
| WithGracefulExceptionHandling | Specifies that any exceptions should be gracefully handled and not thrown after the maximum duration or maximum number of retries have been reached. | false       |
| WithExceptionCallback         | Registers a callback that will be called when an exception of a specified type occurs.                                                               | -           |
| WithPolynomialFactor          | Specifies the exponential factor to use for the polynomial backoff strategy.                                                                         | 2           |




## Backoff strategies
| Strategy    | Description                                                                                                                                                                                                                                                     |
|-------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Linear      | The base delay time is equal to the attempt count.                                                                                                                                                                                                              |
| Exponential | Increases the time between retries exponentially by the retry count.                                                                                                                                                                                            |
| Fibonacci   | The base delay time is returned as the Fibonacci number corresponding to the current attempt.                                                                                                                                                                   |
| Fixed       | The base delay time is calculated as a fixed value.                                                                                                                                                                                                             |
| Polynomial  | The base delay time is calculated as: ae where:a is the number of unsuccessful attempts that have been made, e is the exponent configured for the strategy. The default exponential factor is 2, but can be altered using the `WithPolynomialFactor()` method |

Feedback is highly appreciated. Thank you for choosing TaskEndurer - happy coding!
