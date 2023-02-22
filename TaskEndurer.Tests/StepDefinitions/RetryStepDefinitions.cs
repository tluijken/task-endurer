using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using Xunit;

namespace TaskEndurer.Tests.StepDefinitions;

[Binding]
public class RetryStepDefinitions
{
    private readonly IServiceProvider _serviceProvider;

    public RetryStepDefinitions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [When(@"We execute a task that always fails")]
    public async Task WhenWeExecuteATaskThatAlwaysFails()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            var result = await executor.ExecuteAsync(AlwaysFails).ConfigureAwait(false);
            scenarioContext.Set(result, Constants.TaskResultKey);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }


    private static async Task<bool> AlwaysFails()
    {
        await Task.Delay(Constants.DefaultRetryInterval);
        throw new ApplicationException("Always fails");
    }

    private static async Task AlwaysFailsNoResult()
    {
        await Task.Delay(Constants.DefaultRetryInterval);
        throw new ApplicationException("Always fails");
    }

    private async Task<bool> FailsWithinAllowedRetries()
    {
        await Task.Delay(Constants.DefaultRetryInterval);
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryCount = scenarioContext.ContainsKey(Constants.RetryCountKey)
            ? scenarioContext.Get<int>(Constants.RetryCountKey)
            : 0;

        var maxFailCount = scenarioContext.Get<int>(Constants.MaxFailCountKey);
        if (retryCount >= maxFailCount) return true;

        scenarioContext.Set(retryCount + 1, Constants.RetryCountKey);
        throw new ApplicationException($"Should fail within allowed retries ({maxFailCount})");
    }

    private async Task FailsWithinAllowedRetriesNoResult()
    {
        await Task.Delay(Constants.DefaultRetryInterval);
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryCount = scenarioContext.ContainsKey(Constants.RetryCountKey)
            ? scenarioContext.Get<int>(Constants.RetryCountKey)
            : 0;

        var maxFailCount = scenarioContext.Get<int>(Constants.MaxFailCountKey);
        if (retryCount >= maxFailCount) return;

        scenarioContext.Set(retryCount + 1, Constants.RetryCountKey);
        throw new ApplicationException($"Should fail within allowed retries ({maxFailCount})");
    }

    [When(@"We execute a task that fails (.*) times")]
    public async Task WhenWeExecuteATaskThatFailsTimes(int maxFailCount)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        scenarioContext.Set(maxFailCount, Constants.MaxFailCountKey);
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            var result = await executor.ExecuteAsync(FailsWithinAllowedRetries).ConfigureAwait(false);
            scenarioContext.Set(result, Constants.TaskResultKey);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }


    [Then(@"the task should fail")]
    public void ThenTheTaskShouldFail()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var exception = scenarioContext.Get<Exception>(Constants.RetryExceptionKey);
        Assert.NotNull(exception);
    }

    [Then(@"the task should not fail")]
    public void ThenTheTaskShouldNotFail()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        Assert.False(scenarioContext.ContainsKey(Constants.RetryExceptionKey));
    }


    [When(@"We execute a task without a result that fails (.*) times")]
    public async Task WhenWeExecuteATaskWithoutAResultThatFailsTimes(int maxFailCount)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        scenarioContext.Set(maxFailCount, Constants.MaxFailCountKey);
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            await executor.ExecuteAsync(FailsWithinAllowedRetriesNoResult).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }

    [When(@"We execute a task without a result that always fails")]
    public async Task WhenWeExecuteATaskWithoutAResultThatAlwaysFails()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            await executor.ExecuteAsync(AlwaysFailsNoResult).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }

    [Given(@"We construct a retry policy")]
    public void GivenWeHaveConstructARetryPolicyThatStatesTheMaximumNumberOfRetriesIs()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = RetryPolicyBuilder.Create();
        scenarioContext.Set(retryPolicyBuilder, Constants.RetryPolicyBuilderKey);
    }

    [Given(@"the retry policy has a delay of (.*) second")]
    public void GivenTheRetryPolicyHasADelayOfSecond(int delayInSeconds)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithDelay(TimeSpan.FromSeconds(delayInSeconds));
    }

    [Given(@"a callback is registered to increment a counter on each occurence of the expected exception")]
    public void GivenACallbackIsRegisteredToIncrementACounterOnEachOccurenceOfTheExpectedException()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithExceptionHandling<ApplicationException>(_ =>
        {
            var retryCountValue = scenarioContext.ContainsKey(Constants.IncrementalResultKey)
                ? scenarioContext.Get<int>(Constants.IncrementalResultKey)
                : 0;
            scenarioContext.Set(retryCountValue + 1, Constants.IncrementalResultKey);
        });
    }

    [Then(@"the increment counter should be (.*)")]
    public void ThenTheIncrementCounterShouldBe(int expectedIncrementCount)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryCountValue = scenarioContext.Get<int>(Constants.IncrementalResultKey);
        Assert.Equal(expectedIncrementCount, retryCountValue);
    }

    [Given(@"we build the retry policy")]
    public void GivenWeBuildTheRetryPolicy()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        var retryExecutor = retryPolicyBuilder.Build();
        scenarioContext.Set(retryExecutor, Constants.RetryExecutorKey);
    }

    [Given(@"the retry policy has a maximum number of retries of (.*)")]
    public void GivenTheRetryPolicyHasAMaximumNumberOfRetriesOf(int maximumNumberOfRetries)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithMaxRetries(maximumNumberOfRetries);
    }

    [Given(@"the maximum retry duration is (.*) seconds")]
    public void GivenTheMaximumRetryDurationIsSeconds(int maximumRetryDurationInSeconds)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithMaxDuration(TimeSpan.FromSeconds(maximumRetryDurationInSeconds));
    }

    [Given(@"we allow graceful exception handling")]
    public void GivenWeAllowGracefulExceptionHandling()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithGracefulExceptionHandling();
    }

    [Given(@"the retry policy expects ApplicationExceptions to be thrown")]
    public void GivenTheRetryPolicyExpectsApplicationExceptionsToBeThrown()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithExpectedException<ApplicationException>();
    }

    [Given(@"the retry policy has an exponential backoff policy")]
    public void GivenTheRetryPolicyHasADefaultExponentialBackoffPolicy()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicyBuilder = scenarioContext.Get<RetryPolicyBuilder>(Constants.RetryPolicyBuilderKey);
        retryPolicyBuilder.WithBackoff(BackoffStrategy.Exponential);
    }

    [Given(@"we start measuring the time")]
    public void GivenWeStartMeasuringTheTime()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        scenarioContext.Set(DateTime.Now, Constants.StartTimeKey);
    }

    [Then(@"retry should have taken (.*) seconds")]
    public void ThenRetryShouldHaveTakenAtLeastSeconds(int expectedMinimumDurationInSeconds)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var startTime = scenarioContext.Get<DateTime>(Constants.StartTimeKey);
        var endTime = DateTime.Now;
        var duration = endTime - startTime;
        Assert.Equal(expectedMinimumDurationInSeconds, duration.TotalSeconds, 0);
    }
}