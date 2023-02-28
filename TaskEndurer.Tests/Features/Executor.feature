Feature: Executor feature
Tests executor features

    @Executor
    Scenario: Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration is set in the retry policy when running a task with a result.
        Given we have no maximum duration set for our retry policy
        And We want to use a UntilExpiredRetryExecutor
        When the executor is called with a task that has a result
        Then a NotSupportedException should be thrown
    
    @Executor
    Scenario: Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration is set in the retry policy when running a task without a result.
        Given we have no maximum duration set for our retry policy
        And We want to use a UntilExpiredRetryExecutor
        When the executor is called with a task that has no result
        Then a NotSupportedException should be thrown
        
    @Executor
    Scenario: Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration is set in the retry policy when running a function.
        Given we have no maximum duration set for our retry policy
        And We want to use a UntilExpiredRetryExecutor
        When the executor is called with a function
        Then a NotSupportedException should be thrown
    
    @Executor
    Scenario: Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration is set in the retry policy when running an action.
        Given we have no maximum duration set for our retry policy
        And We want to use a UntilExpiredRetryExecutor
        When the executor is called with an action
        Then a NotSupportedException should be thrown