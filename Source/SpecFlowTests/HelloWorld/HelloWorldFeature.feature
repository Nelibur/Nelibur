Feature: HelloWorldFeature
	In order to find person
	As a BDD fan
	I want to find Gojko

@test
Scenario: Add two numbers
	Given the following persons
	| FirstName | LastName |
	| Martin    | Fowler   |
	| Gojko     | Adzic    |
	And following
	| FirstName | LastName  |
	| Abraham   | Lincoln   |
	| Thomas    | Jefferson |
	When the person start on 'G'
	Then the person was found
	| FirstName | LastName |
	| Gojko     | Adzic    |
