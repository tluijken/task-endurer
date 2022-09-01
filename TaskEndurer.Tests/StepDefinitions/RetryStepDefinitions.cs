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

    private async Task<bool> FailsWithinAllowedRetries()
    {
        await Task.Delay(Constants.DefaultRetryInterval);
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryCount = scenarioContext.ContainsKey(Constants.RetryCountKey) ? scenarioContext.Get<int>(Constants.RetryCountKey) : 0;

        var maxFailCount = scenarioContext.Get<int>(Constants.MaxFailCountKey);
        if (retryCount >= maxFailCount) return true;

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

    [Given(@"We have a retry policy that state the maximum number of retries is (.*)")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumNumberOfRetriesIs(int numberOrRetries)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxRetries(numberOrRetries)
            .ContinueOnException<ApplicationException>(true)
            .Build();

        scenarioContext.Set(retryExecutor, Constants.RetryExecutorKey);
    }

    [Given(@"We have a retry policy with graceful exception handling that state the maximum number of retries is (.*)")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumNumberOfRetriesIsGracefully(int numberOrRetries)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxRetries(numberOrRetries)
            .ContinueOnException<ApplicationException>(true)
            .WithGracefulExceptionHandling()
            .Build();

        scenarioContext.Set(retryExecutor, Constants.RetryExecutorKey);
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

    [Given(@"We have a retry policy that state the maximum duration is (.*) seconds")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumDurationIsSeconds(int maximumDurationInSeconds)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxDuration(TimeSpan.FromSeconds(maximumDurationInSeconds))
            .ContinueOnException<ApplicationException>(true)
            .Build();

        scenarioContext.Set(retryExecutor, Constants.RetryExecutorKey);
    }
}