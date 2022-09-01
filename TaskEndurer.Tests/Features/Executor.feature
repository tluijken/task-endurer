Feature: Executor feature
Tests executor features

    @Executor
    Scenario: Validate that the UntilExpiredRetryExecutor cannot run when no maximum duration is set in the retry policy.
        Given we have no maximum duration set for our retry policy
        And We want to use a UntilExpiredRetryExecutor
        When the executor is called
        Then the task should fail