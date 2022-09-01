using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;
using TechTalk.SpecFlow;

namespace TaskEndurer.Tests.Hooks;

[Binding]
public sealed class InfrastructureHooks
{
    // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var collection = new ServiceCollection();
        return collection;
    }
}
