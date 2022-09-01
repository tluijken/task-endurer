using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskEndurer.Executors;
using TaskEndurer.Helpers;
using TechTalk.SpecFlow;

namespace TaskEndurer.Tests.StepDefinitions;

[Binding]
public class ExecutorStepDefinitions
{
    private readonly IServiceProvider _serviceProvider;

    public ExecutorStepDefinitions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Given(@"We want to use a UntilExpiredRetryExecutor")]
    public void GivenWeWantToUseAUntilExpiredRetryExecutor()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicy = scenarioContext.Get<RetryPolicy>(Constants.RetryPolicyKey);
        scenarioContext.Set(new UntilExpiredRetryExecutor(retryPolicy, new RetryExecutor(retryPolicy)), Constants.RetryExecutorKey);
    }

    [Given(@"we have no maximum duration set for our retry policy")]
    public void GivenWeHaveNoMaximumDurationSetForOurRetryPolicy()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var retryPolicy = new RetryPolicy();
        scenarioContext.Set(retryPolicy, Constants.RetryPolicyKey);
    }

    [When(@"the executor is called with a task that has no result")]
    public async Task WhenTheExecutorIsCalledWithATaskThatHasNoResult()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            await executor.ExecuteAsync(() => Task.CompletedTask).ConfigureAwait(false);
        }
        catch (NotSupportedException e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }
    
    [When(@"the executor is called with a task that has a result")]
    public async Task WhenTheExecutorIsCalledWithATaskThatHasAResult()
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var executor = scenarioContext.Get<IRetryExecutor>(Constants.RetryExecutorKey);
        try
        {
            await executor.ExecuteAsync(() => Task.FromResult(true)).ConfigureAwait(false);
        }
        catch (NotSupportedException e)
        {
            scenarioContext.Set(e, Constants.RetryExceptionKey);
        }
    }
}