Feature: Successfull request
As a registered merchant
When I request a valid payment
Then I should get a successful status 201 and valid payment id

Background: 

Given I am Apple

Scenario: As a merchant, I should receive a successful status 201

	Given I have a valid pending payment
	When I request the payment
	Then I should get a successful status

	
Scenario: As a merchant, I should receive a valid payment id

	Given I have a valid pending payment
	When I request the payment
	Then I should get a valid payment id