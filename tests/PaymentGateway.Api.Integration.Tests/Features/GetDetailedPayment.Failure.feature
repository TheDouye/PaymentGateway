Feature: Query a payment that does not exist
As a merchant
When I query the gateway with an invalid payment id
Then I should receive a payment not found status 404


Background: 

Given I am Apple

Scenario: I query a payment that does not exist

When I query the gateway with a payment id that does not exist
Then I should get a payment not found status
