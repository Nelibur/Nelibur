Feature: Get actions
	In order to find an object instance accordingly with CRUD and message based paradigm
	As a developer
	I want to use Get action

Scenario: Get data with response
	Given the Json service was started
	And I sent data thru Post action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	When I request data by Id '1'
	Then I get data
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	And the Json service was stopped
