Feature: Put actions
	In order to update an object instance accordingly with CRUD and message based paradigm
	As a developer
	I want to use Put action

Scenario: Put data without response
	Given the Json service was started
	And I sent data thru Post action
	| Id | Quantity | ProductId                            |
	| 11 | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	When I update data thru Put action
	| Id | Quantity |
	| 11 | 10       |
	Then the Json service was stopped

Scenario: PutAsync data without response
	Given the Json service was started
	And I sent data thru Post action
	| Id | Quantity | ProductId                            |
	| 10 | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	When I update data thru PutAsync action
	| Id | Quantity |
	| 10 | 10       |
	Then the Json service was stopped
