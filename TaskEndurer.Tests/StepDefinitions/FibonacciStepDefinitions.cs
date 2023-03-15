using System;
using Microsoft.Extensions.DependencyInjection;
using TaskEndurer.Helpers;
using TechTalk.SpecFlow;
using Xunit;

namespace TaskEndurer.Tests.StepDefinitions;

[Binding]
public class FibonacciStepDefinitions
{
    private readonly IServiceProvider _serviceProvider;

    public FibonacciStepDefinitions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Then(@"The number at index (.*) is (.*)")]
    public void ThenTheNumberAtIndexIs(int index, long expectedValue)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        var fibonacciValue = scenarioContext.Get<long>(index.ToString());
        Assert.Equal(expectedValue, fibonacciValue);
    }

    [Given(@"We retrieve a list of (.*) fibonacci numbers")]
    public void GivenWeRetrieveAListOfFibonacciNumbers(uint numberOfItems)
    {
        var scenarioContext = _serviceProvider.GetRequiredService<ScenarioContext>();
        for (uint i = 0; i < numberOfItems; i++)
        {
            var value = Fibonacci.CalculateNumberAtIndex(i);
            scenarioContext.Set(value, i.ToString());
        }
    }
}