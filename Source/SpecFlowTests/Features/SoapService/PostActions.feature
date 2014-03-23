@SoapService
Feature: Post soap actions
	In order to create an object instance accordingly with CRUD and message based paradigm
	As a developer
	I want to use Post action

Scenario: Post data without response
	Given the Soap service was started
	When I send data thru Post action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 91260A16-B8E9-4AA6-AB3E-4C0354F754B3 |
	Then the Soap service was stopped

Scenario: PostAsync data without response
	Given the Soap service was started
	When I send data thru PostAsync action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 91260A16-B8E9-4AA6-AB3E-4C0354F754B3 |
	Then the Soap service was stopped

Scenario: Post data with response
	Given the Soap service was started
	When I send data thru Post with response action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 91260A16-B8E9-4AA6-AB3E-4C0354F754B3 |
	And response equals 'true'
	Then the Soap service was stopped

Scenario: PostAsync data with response
	Given the Soap service was started
	When I send data thru PostAsync with response action
	| Id | Quantity | ProductId                            |
	| 1  | 5        | 91260A16-B8E9-4AA6-AB3E-4C0354F754B3 |
	And response equals 'true'
	Then the Soap service was stopped