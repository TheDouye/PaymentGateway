using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Domain.Tests
{
    public class MerchantTest
    {
        private IPaymentRepository _paymentRepository;
        private IBank _bank;
        private IPaymentIdGenerator _paymentIdGenerator;

        private Merchant _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _bank = Substitute.For<IBank>();
            _paymentIdGenerator = Substitute.For<IPaymentIdGenerator>();
            
            _systemUnderTest = new Merchant("merchant#123", "Apple", _paymentRepository, _bank, _paymentIdGenerator);
        }

        [Test]
        public async Task Should_ReturnPaymentId_When_CashingInPayment()
        {
            //Arrange
            const string expectedPaymentId = "payment#54";
            MockPaymentIdGenerator(expectedPaymentId);
            MockBankProcessing();
            
            //Act
            var actualCashInNewPayment = await _systemUnderTest.CashInNewPayment(new CreditCard(), new Amount());

            //Assert
            actualCashInNewPayment.Id.Should().Be(expectedPaymentId);
        }

        [Test]
        public async Task Should_SaveBankPaymentResult_When_CashingInPayment()
        {
            //Arrange
            const string bankPaymentId = "bank#78";
            MockBankProcessing(bankPaymentId, BankPaymentStatus.Accepted);
            var expectedBankPaymentResult = new BankPaymentResult(bankPaymentId, BankPaymentStatus.Accepted);

            //Act
            var actualCashInNewPayment = await _systemUnderTest.CashInNewPayment(new CreditCard(), new Amount());

            //Assert
            actualCashInNewPayment.BankPaymentResult
                .Should()
                .BeEquivalentTo(expectedBankPaymentResult);
        }

        [Test]
        public async Task Should_SavePayment_When_CashingInPayment()
        {
            //Arrange
            const string paymentId = "payment#666";
            MockBankProcessing();
            MockPaymentIdGenerator(paymentId);

            //Act
            await _systemUnderTest.CashInNewPayment(new CreditCard(), new Amount());

            //Assert
            
            await _paymentRepository.Received(1).AddAsync(Arg.Is<Payment>(payment => payment.Id == paymentId));
        }

        [Test]
        public async Task Should_SetPaymentStatus_When_CashingInPayment()
        {
            //Arrange
            const string expectedPaymentId = "payment#7847";
            MockPaymentIdGenerator(expectedPaymentId);
            MockBankProcessing(bankPaymentStatus: BankPaymentStatus.Accepted);

            //Act
            var actualCashInNewPayment = await _systemUnderTest.CashInNewPayment(new CreditCard(), new Amount());

            //Assert
            actualCashInNewPayment.Status.Should().Be(PaymentStatus.Accepted);
        }


        [Test]
        public async Task Should_SetPaymentStatusFailed_When_BankProcessingFailed()
        {
            //Arrange
            const string expectedPaymentId = "payment#258";
            MockPaymentIdGenerator(expectedPaymentId);
            MockBankProcessingFail(new BankTimedOutException("bank#123", "network issue"));

            //Act
            await ExecuteCashInSafe();

            //Assert
            var payment = GetPaymentFromRepositoryCall();
            payment.Status.Should().Be(PaymentStatus.Failed);
            payment.Error.Should().Be("network issue");
        }

        [Test]
        public async Task Should_SetPaymentStatusFailed_When_UnknownException()
        {
            //Arrange
            const string expectedPaymentId = "payment#258";
            MockPaymentIdGenerator(expectedPaymentId);
            MockBankProcessingFail(new NetworkInformationException(4));

            //Act
            await ExecuteCashInSafe();

            //Assert
            var payment = GetPaymentFromRepositoryCall();
            payment.Status.Should().Be(PaymentStatus.Failed);
            payment.Error.Should().Be("Bank processing failed for unknown reason");
        }
        private async Task ExecuteCashInSafe()
        {
            try
            {
                await _systemUnderTest.CashInNewPayment(new CreditCard(), new Amount());
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void MockBankProcessing(string expectedPaymentId=default, BankPaymentStatus bankPaymentStatus=default)
        {
            _bank
                .ProcessPaymentAsync(new CreditCard(), new Amount())
                .Returns(Task.FromResult(new BankPaymentResult(expectedPaymentId, bankPaymentStatus)));
        }

        private void MockBankProcessingFail(Exception bankTimedOutException)
        {
            _bank
                .ProcessPaymentAsync(new CreditCard(), new Amount())
                .ThrowsForAnyArgs(bankTimedOutException);
        }

        private void MockPaymentIdGenerator(string expectedPaymentId=default)
        {
            _paymentIdGenerator.GeneratePaymentId().Returns(expectedPaymentId);
        }

        private Payment GetPaymentFromRepositoryCall()
        {
            return _paymentRepository.ReceivedCalls().ElementAt(0).GetArguments()[0].As<Payment>();
        }
    }
}