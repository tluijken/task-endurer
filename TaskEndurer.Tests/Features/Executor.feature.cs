﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace TaskEndurer.Tests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class ExecutorFeatureFeature : object, Xunit.IClassFixture<ExecutorFeatureFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Executor.feature"
#line hidden
        
        public ExecutorFeatureFeature(ExecutorFeatureFeature.FixtureData fixtureData, TaskEndurer_Tests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Executor feature", "Tests executor features", ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a task with a result.")]
        [Xunit.TraitAttribute("FeatureTitle", "Executor feature")]
        [Xunit.TraitAttribute("Description", "Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a task with a result.")]
        [Xunit.TraitAttribute("Category", "Executor")]
        public void ValidateThatTheUntilExpiredRetryExecutorCannotRunWhenNoMaximumDurationIsSetInTheRetryPolicyWhenRunningATaskWithAResult_()
        {
            string[] tagsOfScenario = new string[] {
                    "Executor"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
                    "s set in the retry policy when running a task with a result.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 5
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
        testRunner.Given("we have no maximum duration set for our retry policy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 7
        testRunner.And("We want to use a UntilExpiredRetryExecutor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 8
        testRunner.When("the executor is called with a task that has a result", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 9
        testRunner.Then("a NotSupportedException should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a task without a result.")]
        [Xunit.TraitAttribute("FeatureTitle", "Executor feature")]
        [Xunit.TraitAttribute("Description", "Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a task without a result.")]
        [Xunit.TraitAttribute("Category", "Executor")]
        public void ValidateThatTheUntilExpiredRetryExecutorCannotRunWhenNoMaximumDurationIsSetInTheRetryPolicyWhenRunningATaskWithoutAResult_()
        {
            string[] tagsOfScenario = new string[] {
                    "Executor"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
                    "s set in the retry policy when running a task without a result.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 12
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 13
        testRunner.Given("we have no maximum duration set for our retry policy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 14
        testRunner.And("We want to use a UntilExpiredRetryExecutor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 15
        testRunner.When("the executor is called with a task that has no result", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 16
        testRunner.Then("a NotSupportedException should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a function.")]
        [Xunit.TraitAttribute("FeatureTitle", "Executor feature")]
        [Xunit.TraitAttribute("Description", "Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running a function.")]
        [Xunit.TraitAttribute("Category", "Executor")]
        public void ValidateThatTheUntilExpiredRetryExecutorCannotRunWhenNoMaximumDurationIsSetInTheRetryPolicyWhenRunningAFunction_()
        {
            string[] tagsOfScenario = new string[] {
                    "Executor"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
                    "s set in the retry policy when running a function.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 19
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 20
        testRunner.Given("we have no maximum duration set for our retry policy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 21
        testRunner.And("We want to use a UntilExpiredRetryExecutor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 22
        testRunner.When("the executor is called with a function", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 23
        testRunner.Then("a NotSupportedException should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running an action.")]
        [Xunit.TraitAttribute("FeatureTitle", "Executor feature")]
        [Xunit.TraitAttribute("Description", "Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
            "s set in the retry policy when running an action.")]
        [Xunit.TraitAttribute("Category", "Executor")]
        public void ValidateThatTheUntilExpiredRetryExecutorCannotRunWhenNoMaximumDurationIsSetInTheRetryPolicyWhenRunningAnAction_()
        {
            string[] tagsOfScenario = new string[] {
                    "Executor"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration i" +
                    "s set in the retry policy when running an action.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 26
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 27
        testRunner.Given("we have no maximum duration set for our retry policy", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 28
        testRunner.And("We want to use a UntilExpiredRetryExecutor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 29
        testRunner.When("the executor is called with an action", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 30
        testRunner.Then("a NotSupportedException should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ExecutorFeatureFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ExecutorFeatureFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
