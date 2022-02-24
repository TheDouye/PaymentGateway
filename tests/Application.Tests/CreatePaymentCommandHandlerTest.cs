using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Application.Tests
{
    public class PaymentGatewayControllerTest
    {
        private IMerchantRepository _merchantRepository;

        private CreatePaymentCommandHandler _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _merchantRepository = Substitute.For<IMerchantRepository>();

            _systemUnderTest = new CreatePaymentCommandHandler(_merchantRepository, Substitute.For<ILogger<CreatePaymentCommandHandler>>());
        }

        [Test]
        public async Task Should_CashInMerchantPayment_When_CommandReceived()
        {
            //Arrange
            const string expectedPaymentId = "payment#145";
            const string merchantId = "7da98d82-5f77-46d9-9531-5472779e1f97";
            var createPaymentCommand = CreatePaymentCommand(merchantId);
            var merchant = Substitute.For<IMerchant>();
            MockMerchant(merchant, expectedPaymentId);
            MockMerchantRepository(merchantId, merchant);
            
            //Act
            var actualPaymentResult = await _systemUnderTest.Handle(createPaymentCommand, CancellationToken.None);

            //Assert
            actualPaymentResult.PaymentId.Should().Be(expectedPaymentId);
        }

        private static void MockMerchant(IMerchant merchant, string expectedPaymentId)
        {
            var payment = new Payment(expectedPaymentId, new CreditCard(), new Amount());

            merchant.CashInNewPayment(new CreditCard(), new Amount()).Returns(Task.FromResult(payment));
        }

        private void MockMerchantRepository(string merchantId, IMerchant merchant)
        {
            _merchantRepository.GetMerchantByIdAsync(merchantId).Returns(Task.FromResult(merchant));
        }

        private static CreatePaymentCommand CreatePaymentCommand(string merchantId)
        {
            return new CreatePaymentCommand
            {
                MerchantId = merchantId, 
                Amount = new Amount(), 
                CreditCard = new CreditCard()
            };
        }
    }
}
