Feature: Query a payment that exists
As a merchant
When I query the gateway with a valid payment id
Then I should get a detailed response

Background: 

Given I am Apple

Scenario: I should receive a successful status 200

	Given I did a payment
	When I query the gateway with the payment id
	Then I should get a successful status 200

Scenario: I should receive a masked card number

	Given I did a payment with card number 1258 5214 6587 3254
	When I query the gateway with the payment id
	Then I should get a detailed response with a masked card number 1258 XXXX XXXX 3254

Scenario: I should receive the payment status

	Given I did a payment
	When I query the gateway with the payment id
	Then I should get the payment status

Scenario: I should receive the card details

	Given I did a payment
	When I query the gateway with the payment id
	Then I should get the card details