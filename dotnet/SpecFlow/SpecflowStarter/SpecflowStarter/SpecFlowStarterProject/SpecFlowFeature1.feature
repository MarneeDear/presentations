Feature: SpecFlowFeature1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

@usersgrouptest
Scenario: Marnee gives a successful presentation
	Given user Marnee is presenting SpecFlow
		| User   | Topic                                                  |
		| Marnee | Using SpecFlow for Behavior Driven Development in .Net |
	When Marnee gives the talk
	Then we learn about Behavior Driven Development and SpecFlow

