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
        
    Scenario: Retry a task for a maximum of 3 times with a 1 second delay which increments a counter on each occurence of the expected exception
        Given We have construct a retry policy that states the maximum number of retries is 3
        And the retry policy has a delay of 1 second
        And a callback is registered to increment a counter on each occurence of the expected exception
        And we build the retry policy
        When We execute a task that always fails
        Then the increment counter should be 3