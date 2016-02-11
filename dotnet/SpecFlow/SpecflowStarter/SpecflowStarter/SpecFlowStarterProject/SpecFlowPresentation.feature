Feature: SpecFlowPresentation
	.Net Users Group Behavior Driven Development with SpecFlow 

Background: 

@usersgrouptest
Scenario: Marnee gives a successful presentation
	Given user Marnee is presenting SpecFlow
		| PresenterName   | Topic                                                  |
		| Marnee | Using SpecFlow for Behavior Driven Development in .Net |
	When Marnee gives the talk to the group
		| GroupName            |
		| .Net Users group |
	Then we learn about Behavior Driven Development and SpecFlow
		
