Feature: RetryFeature
Testing various retry policies

    @retryPolicy
    Scenario: Retry a task with a result a for a maximum amount of retries unsuccessfully
        Given We have a retry policy that state the maximum number of retries is 3
        When We execute a task that always fails
        Then the task should fail

    Scenario: Retry a task with a result a for a maximum amount of retries successfully
        Given We have a retry policy that state the maximum number of retries is 3
        When We execute a task that fails 2 times
        Then the task should not fail

    Scenario: Retry a task with a result a for a maximum duration unsuccessfully
        Given We have a retry policy that state the maximum duration is 3 seconds
        When We execute a task that always fails
        Then the task should fail

    Scenario: Retry a task with a result a for a maximum duration successfully
        Given We have a retry policy that state the maximum duration is 3 seconds
        When We execute a task that fails 10 times
        Then the task should not fail
        
    Scenario: Retry a task without a result a for a maximum amount of retries unsuccessfully
        Given We have a retry policy that state the maximum number of retries is 3
        When We execute a task without a result that always fails
        Then the task should fail

    Scenario: Retry a task without a result a for a maximum amount of retries successfully
        Given We have a retry policy that state the maximum number of retries is 3
        When We execute a task without a result that fails 2 times
        Then the task should not fail

    Scenario: Retry a task without a result a for a maximum duration unsuccessfully
        Given We have a retry policy that state the maximum duration is 3 seconds
        When We execute a task without a result that always fails
        Then the task should fail

    Scenario: Retry a task without a result a for a maximum duration successfully
        Given We have a retry policy that state the maximum duration is 3 seconds
        When We execute a task without a result that fails 10 times
        Then the task should not fail

    Scenario: Retry a task a for a maximum amount of retries with graceful exception handling
        Given We have a retry policy with graceful exception handling that state the maximum number of retries is 3
        When We execute a task that always fails
        Then the task should not fail
        When We execute a task that always fails
        Then the task should not fail