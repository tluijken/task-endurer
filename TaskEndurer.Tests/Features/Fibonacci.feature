Feature: Fibonacci
	Validates our fibonacci calculations

@Fibonacci
Scenario: Get the first 10 fibonacci numbers
	Given We retrieve a list of 10 fibonacci numbers
	Then The number at index 0 is 0
	And The number at index 1 is 1
	And The number at index 2 is 1
	And The number at index 3 is 2
	And The number at index 4 is 3
	And The number at index 5 is 5
	And The number at index 6 is 8
	And The number at index 7 is 13
	And The number at index 8 is 21
	And The number at index 9 is 34