Feature: Bank processing failure

As a registered merchant
When I request a payment the bank can not handle
Then I should get a payment response with a Failed status


Background: 

Given I am Abble

Scenario: The bank times out but I should get a status 201

	Given I have a valid pending payment
	When I request the payment
	Then I should get a successful status

	Scenario: The bank times out, the payment should have been saved with a failed status

	Given I have a valid pending payment
	When I request the payment
	Then the payment must have failed
