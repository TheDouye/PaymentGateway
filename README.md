# PaymentGateway

## How to run the solution
As you already work with dotnet 5, it should be easy:
  - Open the sln file with Visual Studio 2019 and set PaymentGateway.Api as startup project. Debug the program.
  - Use `dotnet PaymentGateway.Api.exe` command to run the program without debugging

The program will listen requests at https://localhost:5001 and you should be redirected to the swagger home page

## Assumptations made
- The idea of the home work was to focus on the API side and the different use cases. That's why:
  - Many services are mocked and dummy implementations are provided. Examples: `DateServiceMock` to provide current date, `CurrencyServiceMock` to provide ISO currencies referential, `PaymentRepository` to save payments and so on.
  - Simple workflow: no domain events that could affect a payment status. I can implement something more sophisticated if you judge it relevant.
  - Focus on testability:
    - Unit tests for most of the classes
    - Behaviour / Integration tests with SpecFlow: you will find in Documentation folder PaymentGateway.LivingDocumentation.html that you can download, and visualize the different test cases using gherkin scripts. The file was generated via the SpecFlow CLI and could be part of any CI. The scenarios could be improved but I did not have my two Amigos to write them ;). Requests are sent to an endpoint built with a `WebApplicationFactory`. See class `RequestPaymentStepDefinitions`. 
  - Clean architecture and BDD approaches used to underly the different use cases

## Areas for improvements
- Exception handling: as a first step, I handled them in an `ExceptionFilter` implementation but its responsibility seems huge. I let some comments in `Merchant` class
- I use the request to hold the MerchantId: I think it's not a good practice. That should be automatically added to the meta data. I can do it if needed.
- Reroute to another url using the payment id after processing a payment
- Use a kind of unified API result to make its usage easier
- Write more readable gherkin
- Add more logs using a cross cutting approach. Current logs make of course use of structured logging capabilities from NLOG (no configuration available, the log file is in the output folder)
- Add authentification
- Add audit
- Add metrics: performance, monitoring (health check)
- ...

## Workflow
- Requests are handled by the Controller
- CQRS using MediatR. Commands and queries are handled by a simple pipeline: validation and handler
- Results are returned asynchronously

## Cloud technologies
As discussed with Clara, I m not an expert but if I project myself and having understood what you seem to use (reading the job description and so on):
- Serverless with Amazon
- A NoSql database for the queries. Example: DynamoDb
- Some ETL tools like AWS Glue
- Inter process communication: for instance we could image the different Bank adapters are in other services and would communicate with the gateway or other services via Amazon SNS or SQS depending on the needs (publish-subscribe, message queue for ordering and so on)



