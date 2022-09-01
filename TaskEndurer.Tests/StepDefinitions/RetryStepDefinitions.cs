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
    private const string MaxFailCountKey = "maxRetryCount";
    private const string RetryCountKey = "retryCount";
    private const string RetryExecutorKey = "retryExecutor";
    private const string RetryExceptionKey = "retryException";
    private const string TaskResultKey = "taskResult";
    private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromMilliseconds(100);

    public RetryStepDefinitions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [When(@"We execute a task that always fails")]
    public async Task WhenWeExecuteATaskThatAlwaysFails()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var executor = scenarioContext.Get<IRetryExecutor>(RetryExecutorKey);
        try
        {
            var result = await executor.ExecuteAsync(AlwaysFails).ConfigureAwait(false);
            scenarioContext.Set(result, TaskResultKey);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, RetryExceptionKey);
        }
    }


    private static async Task<bool> AlwaysFails()
    {
        await Task.Delay(DefaultRetryInterval);
        throw new ApplicationException("Always fails");
    }

    private async Task<bool> FailsWithinAllowedRetries()
    {
        await Task.Delay(DefaultRetryInterval);
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryCount = scenarioContext.ContainsKey(RetryCountKey) ? scenarioContext.Get<int>(RetryCountKey) : 0;

        var maxFailCount = scenarioContext.Get<int>(MaxFailCountKey);
        if (retryCount >= maxFailCount) return true;

        scenarioContext.Set(retryCount + 1, RetryCountKey);
        throw new ApplicationException($"Should fail within allowed retries ({maxFailCount})");
    }

    [When(@"We execute a task that fails (.*) times")]
    public async Task WhenWeExecuteATaskThatFailsTimes(int maxFailCount)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        scenarioContext.Set(maxFailCount, MaxFailCountKey);
        var executor = scenarioContext.Get<IRetryExecutor>(RetryExecutorKey);
        try
        {
            var result = await executor.ExecuteAsync(FailsWithinAllowedRetries).ConfigureAwait(false);
            scenarioContext.Set(result, TaskResultKey);
        }
        catch (Exception e)
        {
            scenarioContext.Set(e, RetryExceptionKey);
        }
    }

    [Given(@"We have a retry policy that state the maximum number of retries is (.*)")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumNumberOfRetriesIs(int numberOrRetries)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxRetries(numberOrRetries)
            .ContinueOnException<ApplicationException>(true)
            .Build();
        
        scenarioContext.Set(retryExecutor, RetryExecutorKey);
    }
    
    [Given(@"We have a retry policy with graceful exception handling that state the maximum number of retries is (.*)")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumNumberOfRetriesIsGracefully(int numberOrRetries)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxRetries(numberOrRetries)
            .ContinueOnException<ApplicationException>(true)
            .WithGracefulExceptionHandling()
            .Build();
        
        scenarioContext.Set(retryExecutor, RetryExecutorKey);
    }

    [Then(@"the task should fail")]
    public void ThenTheTaskShouldFail()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var exception = scenarioContext.Get<Exception>(RetryExceptionKey);
        Assert.NotNull(exception);
        
    }

    [Then(@"the task should not fail")]
    public void ThenTheTaskShouldNotFail()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        Assert.False(scenarioContext.ContainsKey(RetryExceptionKey));
    }

    [Given(@"We have a retry policy that state the maximum duration is (.*) seconds")]
    public void GivenWeHaveARetryPolicyThatStateTheMaximumDurationIsSeconds(int maximumDurationInSeconds)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryExecutor = RetryPolicyBuilder.Create().WithMaxDuration(TimeSpan.FromSeconds(maximumDurationInSeconds))
            .ContinueOnException<ApplicationException>(true)
            .Build();
        
        scenarioContext.Set(retryExecutor, RetryExecutorKey);
    }
}
