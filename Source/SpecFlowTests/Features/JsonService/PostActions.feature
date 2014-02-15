﻿Feature: Post actions
	In order to create an object instance accordingly with CRUD and message based paradigm
	As a developer
	I want to use Post action

Scenario: Post data without response
	Given the Json service was started
	When I send data thru Post action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 5B1706AC-F33C-43B5-8ACF-BAEB2E73BB95 |
	Then the Json service was stopped
