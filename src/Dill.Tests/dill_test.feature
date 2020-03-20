Feature: Test other features

  Scenario: Test optional parameters when null
    Given foo
	When foo does bar
	Then bar is foo

  Scenario: Test optional parameters when supplied
    Given foo
	When foo does bar twice
	Then bar is foo