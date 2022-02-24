Feature: Request a payment while I am not declare to checkout.com

As a merchant
If I have not been onboarded by checkout.com
When I request a payment
Then I should always receive a unauthorized response

Scenario: As a merchant, I should be registered to checkout.com in order to request the gateway
	Given I am Nobody
	Given I have a valid pending payment
	When I request the payment
	Then I should get an unauthorized response
