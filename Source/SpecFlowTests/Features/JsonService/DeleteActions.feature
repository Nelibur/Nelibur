Feature: Delete actions
	In order to delete an object instance accordingly with CRUD and message based paradigm
	As a developer
	I want to use Delete action

Scenario: Delete data without response
	Given the Json service was started
	And I sent data thru Post action
	| Id | Quantity | ProductId                            |
	| 3  | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	When I send delete request by Id '3' thru Delete action
	Then the Json service was stopped

Scenario: DeleteAsync data without response
	Given the Json service was started
	And I sent data thru Post action
	| Id | Quantity | ProductId                            |
	| 4  | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	When I send delete request by Id '4' thru DeleteAsync action
	Then the Json service was stopped
