using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Queries;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentGateway.Api.Dtos;
using TechTalk.SpecFlow;

namespace PaymentGateway.Api.Integration.Tests.StepDefinitions
{
    [Binding]
    public sealed class RequestPaymentStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private IMerchantRepository _merchantRepository;

        public RequestPaymentStepDefinitions(ScenarioContext scenarioContext, WebApplicationFactory<PaymentGatewayStartup> webApplicationFactory)
        {
            _scenarioContext = scenarioContext;

            //_merchantRepository = merchantRepository;

            // Escape exceptions on SSL and so on, out of the scope of these tests
            var httpClient = webApplicationFactory.CreateClient();

            _merchantRepository = webApplicationFactory.Services.GetRequiredService<IMerchantRepository>();
            
            _scenarioContext.Set(httpClient);
            
        }

        [Given(@"I am (.*)")]
        public async Task GivenIAmThisMerchant(string merchant)
        {
            var foundMerchant = await _merchantRepository.GetMerchantByNameOrDefaultAsync(merchant);

            _scenarioContext.Set<IMerchant>(foundMerchant);
        }

        [Given(@"I provide an invalid merchant identifier")]
        public void GivenIProvideAnInvalidMerchantIdentifier()
        {

            SetPaymentCommand(1000, "EUR", "1111 2222 3333 4444", 12, 2022, "169");
        }


        [Given(@"My shopper entered an invalid card number")]
        public void GivenIProvideAnInvalidCardNumber()
        {
            const string invalidCardNumber = "*1111 2222 3333 4444*";

            SetPaymentCommand(1000, "EUR", invalidCardNumber, 12, 2022, "123");
        }

        [Given(@"My shopper's card is expired")]
        public void GivenIProvideAnExpiredCard()
        {
            const int expiredCardYear = 1983;

            SetPaymentCommand(1000, "EUR", "1111 0000 2222 4444", 12, expiredCardYear, "123");
        }

        [Given(@"My shopper entered an invalid cvv")]
        public void GivenIProvideAnInvalidCardCvv()
        {
            const string invalidCardCvv = "1236";

            SetPaymentCommand(1000, "EUR", "1111 0000 2222 4444", 12, 2040, invalidCardCvv);
        }

        [Given(@"My shopper entered an invalid currency")]
        public void GivenIProvideAnInvalidPaymentCurrency()
        {
            const string invalidCurrency = "XXX";

            SetPaymentCommand(1000, invalidCurrency, "1111 0000 2222 4444", 12, 2040, "1236");
        }

        [Given(@"I have a valid pending payment")]
        public void GivenIHaveAValidPendingPayment()
        {
            SetPaymentCommand(1000, "EUR", "1111 2222 3333 4444", 03, 2022, "666");
        }

        [Given(@"I did a payment")]
        public async Task GivenIRequestedASuccessfulPayment()
        {
            SetPaymentCommand(1000, "EUR", "1111 2222 3333 4444", 03, 2022, "678");
            await WhenIRequestThePayment();
        }

        [Given(@"I did a payment with card number (.*)")]
        public async Task GivenIDidASuccessfulPaymentWithCardNumber(string cardNumber)
        {
            SetPaymentCommand(1000, "EUR", cardNumber, 03, 2022, "666");
            await WhenIRequestThePayment();
        }

        [When(@"I request the payment")]
        public async Task WhenIRequestThePayment()
        {
            var paymentRequest = _scenarioContext.Get<CreatePayment>();

            var requestContent = JsonConvert.SerializeObject(paymentRequest);

            var httpResponseMessage = await _scenarioContext.Get<HttpClient>()
                .PostAsync("api/payments", new StringContent(requestContent, Encoding.UTF8, "application/json"));

            SetCurrentHttpResponse(httpResponseMessage);
        }

        [When(@"I query the gateway with the payment id")]
        public async Task WhenIQueryTheGatewayWithTheLastPaymentId()
        {
            var currentPaymentId = (await GetCurrentPaymentId()).Replace("\"", string.Empty);

            await QueryThePaymentAndSaveTheResponse(currentPaymentId);
        }

        [When(@"I query the gateway with a payment id that does not exist")]
        public async Task WhenIQueryTheGatewayWithAnInvalidPaymentId()
        {
            await QueryThePaymentAndSaveTheResponse("ThisPaymentIdDoeNotExistForSure");
        }

        [Then(@"I should get a payment not found status")]
        public void ThenIShouldGetAPaymentNotFoundStatus()
        {
            GetCurrentHttpStatusCode().Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"I should get an unprocessable entity response")]
        public void ThenIShouldGetABadRequestResult()
        {
            var currentHttpResponseMessage = GetCurrentHttpResponseMessage();

            currentHttpResponseMessage.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Then(@"I should get an unauthorized response")]
        public void ThenIShouldGetAnUnauthorizedResponse()
        {
            GetCurrentHttpStatusCode().Should().Be(HttpStatusCode.Unauthorized);
        }

        [Then(@"I should get a successful status")]
        public void ThenIShouldGetASuccessfulStatus()
        {
            GetCurrentHttpStatusCode().Should().Be(HttpStatusCode.Created);
        }

        [Then(@"I should get a valid payment id")]
        public async Task ThenIShouldGetAValidPaymentId()
        {
            var result = await GetCurrentPaymentId();

            result.Should().NotBeNullOrEmpty();
        }

        [Then(@"I should get a successful status 200")]
        public void ThenIShouldGetADetailedPaymentInformation()
        {
            GetCurrentHttpStatusCode().Should().Be(HttpStatusCode.OK);
        }

        [Then(@"I should get a detailed response with a masked card number (.*)")]
        public async Task ThenIShouldGetAMaskCardNumber(string maskCardNumber)
        {
            var detailedPayment = await GetCurrentDetailedPayment();

            detailedPayment.CreditCard.Number.Should().Be(maskCardNumber);
        }

        [Then(@"I should get the payment status")]
        public async Task ThenIShouldGetThePaymentStatus()
        {
            var currentDetailedPayment = await GetCurrentDetailedPayment();

            currentDetailedPayment.Status.Should().Be("Accepted");
        }

        [Then(@"I should get the card details")]
        public async Task ThenIShouldGetTheCardDetails()
        {
            var currentDetailedPayment = await GetCurrentDetailedPayment();
            
            currentDetailedPayment.CreditCard.Cvv.Should().Be("678");
            currentDetailedPayment.CreditCard.Expiry.Month.Should().Be(3);
            currentDetailedPayment.CreditCard.Expiry.Year.Should().Be(2022);
        }

        [Then(@"the payment must have failed")]
        public async Task ThenThePaymentMustFail()
        {
            await WhenIQueryTheGatewayWithTheLastPaymentId();

            var currentDetailedPayment = await GetCurrentDetailedPayment();

            currentDetailedPayment.Status.Should().Be("Failed");
        }
        
        private async Task<string> GetCurrentPaymentId()
        {
            return await GetCurrentHttpResponseMessage()
                .Content
                .ReadAsStringAsync();
        }

        private void SetPaymentCommand(decimal amount, string currency, string cardNumber, int cardExpiryMonth, int cardExpiryYear, string cardCvv)
        {
            var currentMerchant = GetCurrentMerchant();
            var paymentCommand = new CreatePayment
            {
                Amount = amount,
                CreditCardCvv = cardCvv,
                CreditCardExpiryMonth = cardExpiryMonth,
                CreditCardExpiryYear = cardExpiryYear,
                CreditCardNumber = cardNumber,
                Currency = currency,
                MerchantId = currentMerchant?.Id
            };

            _scenarioContext.Set(paymentCommand);
        }

        private void SetCurrentHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            _scenarioContext.Set(httpResponseMessage);
        }

        private HttpResponseMessage GetCurrentHttpResponseMessage()
        {
            return _scenarioContext.Get<HttpResponseMessage>();
        }

        private async Task QueryThePaymentAndSaveTheResponse(string currentPaymentId)
        {
            var httpResponseMessage = await _scenarioContext.Get<HttpClient>().GetAsync($"api/payments/{currentPaymentId}");

            SetCurrentHttpResponse(httpResponseMessage);
        }

        private HttpStatusCode GetCurrentHttpStatusCode()
        {
            var currentHttpResponseMessage = GetCurrentHttpResponseMessage();

            return currentHttpResponseMessage.StatusCode;
        }

        private IMerchant GetCurrentMerchant()
        {
            return _scenarioContext.Get<IMerchant>();
        }

        private async Task<DetailedPayment> GetCurrentDetailedPayment()
        {
            return JsonConvert.DeserializeObject<DetailedPayment>(await GetCurrentHttpResponseMessage().Content.ReadAsStringAsync());
        }
    }
}