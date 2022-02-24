Feature: Bad request result

As a registered merchant
When I request an invalid payment
Then I should get an unprocessable entity response

Background: 

Given I am Apple

Scenario: As a merchant, I should provide a valid card number
	
	Given My shopper entered an invalid card number
	When I request the payment
	Then I should get an unprocessable entity response

Scenario: As a merchant, I should provide a non expired card
	
	Given My shopper's card is expired
	When I request the payment
	Then I should get an unprocessable entity response

Scenario: As a merchant, I should provide a valid card cvv
	
	Given My shopper entered an invalid cvv
	When I request the payment
	Then I should get an unprocessable entity response

Scenario: As a merchant, I should provide an ISO payment currency
	
	Given My shopper entered an invalid currency
	When I request the payment
	Then I should get an unprocessable entity response