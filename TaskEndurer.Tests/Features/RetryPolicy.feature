Feature: RetryFeature
Testing various retry policies

    @retryPolicy
    Scenario: Retry a task with a result a for a maximum amount of retries unsuccessfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task that always fails
        Then the task should fail

    Scenario: Retry a task with a result a for a maximum amount of retries successfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task that fails 2 times
        Then the task should not fail

    Scenario: Retry a task with a result a for a maximum duration unsuccessfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task that always fails
        Then the task should fail

    Scenario: Retry a task with a result a for a maximum duration successfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task that fails 10 times
        Then the task should not fail
        
    Scenario: Retry a task without a result a for a maximum amount of retries unsuccessfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task without a result that always fails
        Then the task should fail
    
    Scenario: Retry a function a for a maximum amount of retries unsuccessfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a function with a result that fails 10 times
        Then the task should fail

    Scenario: Retry an action a for a maximum amount of retries unsuccessfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute an action that fails 10 times
        Then the task should fail
        
    Scenario: Retry a task without a result a for a maximum amount of retries successfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task without a result that fails 2 times
        Then the task should not fail

    Scenario: Retry a task without a result a for a maximum duration unsuccessfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task without a result that always fails
        Then the task should fail

    Scenario: Retry a task without a result a for a maximum duration successfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a task without a result that fails 10 times
        Then the task should not fail

    Scenario: Retry a task a for a maximum amount of retries with graceful exception handling
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we allow graceful exception handling
        And we build the retry policy
        When We execute a task that always fails
        Then the task should not fail
        When We execute a task that always fails
        Then the task should not fail
      
    Scenario: Retry a task without a return value a for a maximum amount of retries with graceful exception handling
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we allow graceful exception handling
        And we build the retry policy
        When We execute a task without a result that always fails
        Then the task should not fail
        When We execute a task that always fails
        Then the task should not fail
    
    Scenario: Retry an action without a return value a for a maximum amount of retries with graceful exception handling
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we allow graceful exception handling
        And we build the retry policy
        When We execute an action that fails 10 times
        Then the task should not fail
        When We execute a task that always fails
        Then the task should not fail
    
    Scenario: Retry a function a for a maximum amount of retries with graceful exception handling
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we allow graceful exception handling
        And we build the retry policy
        When We execute a function with a result that fails 10 times
        Then the task should not fail
        When We execute a task that always fails
        Then the task should not fail

    Scenario: Retry a task for a maximum of 3 times with a 1 second delay which increments a counter on each occurence of the expected exception
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy has a delay of 1 second
        And a callback is registered to increment a counter on each occurence of the expected exception
        And we build the retry policy
        When We execute a task that always fails
        Then the increment counter should be 3
    
    Scenario: Retry a task with the exponential backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an exponential backoff policy
        And the retry policy has a delay of 1 second
        And we build the retry policy
        And we start measuring the time
        When We execute a task that always fails
        # (1x1)1 + (2x2)4 + (3x3)9 = 14 seconds
        Then retry should have taken 14 seconds
        
    Scenario: Retry a task with the linear backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an linear backoff policy
        And the retry policy has a delay of 1 second
        And we build the retry policy
        And we start measuring the time
        When We execute a task that always fails
        # 1 + 2 + 3 = 5 seconds
        Then retry should have taken 5 seconds
        
    Scenario: Retry a task with the fibonacci backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 5
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an fibonacci backoff policy
        And the retry policy has a delay of 1 second
        And we build the retry policy
        And we start measuring the time
        When We execute a task that always fails
        # (0 skipped) + 1 + 1 + 2 + 3 + 5 = 12 seconds
        Then retry should have taken 12 seconds
        
    Scenario: Retry a task with the fixed backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 5
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an fixed backoff policy
        And the retry policy has a delay of 1 second
        And we build the retry policy
        And we start measuring the time
        When We execute a task that always fails
        Then retry should have taken 5 seconds
        
    Scenario: Retry a task with the polynomial backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an polynomial backoff policy with a polynomial factor of 3
        And the retry policy has a delay of 1 second
        And we build the retry policy
        And we start measuring the time
        When We execute a task that always fails
        # (1x1³)1 + (2x2³)8 + (3x3³)27 = 36 seconds
        Then retry should have taken 36 seconds
        
    Scenario: Retry a function with a result a for a maximum amount of retries successfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a function with a result that fails 2 times
        Then result should be set to true
        
    Scenario: Retry an action a for a maximum amount of retries successfully
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute an action that fails 2 times
        Then the task should not fail
        
    Scenario: Retry a function with a result a for a maximum duration successfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute a function with a result that fails 2 times
        Then result should be set to true
        
    Scenario: Retry an action a for a maximum duration successfully
        Given We construct a retry policy
        And the maximum retry duration is 3 seconds
        And the retry policy expects ApplicationExceptions to be thrown
        And we build the retry policy
        When We execute an action that fails 2 times
        Then the task should not fail
        
    Scenario: Retry a task with the polynomial backoff policy and a negative polynomial factor
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an polynomial backoff policy with a polynomial factor of -3
        And the retry policy has a delay of 1 second
        Then an ArgumentOutOfRangeException should be thrown
        
    Scenario: Retry a task with the polynomial backoff policy and a polynomial factor of zero
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an polynomial backoff policy with a polynomial factor of 0
        And the retry policy has a delay of 1 second
        Then an ArgumentOutOfRangeException should be thrown
        
    Scenario: Retry a task with the invalid backoff policy
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And the retry policy expects ApplicationExceptions to be thrown
        And the retry policy has an invalid backoff policy
        And the retry policy has a delay of 1 second
        And we build the retry policy
        When We execute a function with a result that fails 2 times
        Then an ArgumentOutOfRangeException should be thrown
        
    Scenario: Retry a task without specifying which exceptions to expect
        Given We construct a retry policy
        And the retry policy has a maximum number of retries of 3
        And we build the retry policy
        When We execute an action that fails 2 times
        Then the task should fail
       