using System.Threading.Tasks;
using Application.Services;
using Application.Validators;
using Domain;
using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace Application.Tests
{
    internal class PaymentAmountValidatorTest
    {
        private ICurrencyService _currencyService;

        private PaymentAmountValidator _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _currencyService = Substitute.For<ICurrencyService>();

            _systemUnderTest = new PaymentAmountValidator(_currencyService);
        }

        [TestCase(0)]
        [TestCase(-10)]
        public async Task Should_BeInValid_When_AmountLowerOrEqualToZero(decimal amount)
        {
            //Arrange
            MockCurrencyService("EUR", true);

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(Amount.Create(amount, "EUR"));

            //Assert
            ShouldContainErrorMessage(validationResult, "'Amount' must be greater than '0'.");
        }

        [Test]
        public async Task Should_BeInValid_When_CurrencyNotIso()
        {
            //Arrange

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(Amount.Create(1000, "XYZ"));

            //Assert
            ShouldContainErrorMessage(validationResult, "Currency not ISO");
        }

        private void MockCurrencyService(string currencyName, bool isIso)
        {
            _currencyService.IsIsoCurrency(currencyName).Returns(Task.FromResult(isIso));
        }

        private static void ShouldContainErrorMessage(ValidationResult validationResult, string errorMessage)
        {
            validationResult.Errors.Should().Contain(failure => failure.ErrorMessage == errorMessage);
        }
    }
}