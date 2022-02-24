using System.Threading;
using Application.Commands;
using Application.Validators;
using Domain;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using NSubstitute;
using NUnit.Framework;

namespace Application.Tests
{
    internal class CreatePaymentCommandValidatorTest
    {
        private IValidator<CreditCard> _creditCardValidator;
        private IValidator<Amount> _paymentAmountValidator;

        private CreatePaymentCommandValidator _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _creditCardValidator = Substitute.For<IValidator<CreditCard>>();
            _paymentAmountValidator = Substitute.For<IValidator<Amount>>();
            
            _systemUnderTest = new CreatePaymentCommandValidator(_creditCardValidator, _paymentAmountValidator);
        }

        [Test]
        public void Should_BeValid_When_AllPropertiesValid()
        {
            //Arrange

            //Act
            var validationResult = _systemUnderTest.Validate(new CreatePaymentCommand());

            //Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Test]
        public void Should_NotBeValid_When_CreditCardInvalid()
        {
            //Arrange
            var invalidCommand = new CreatePaymentCommand
            {
                CreditCard = new CreditCard()
            };
            _creditCardValidator.Validate(Arg.Any<IValidationContext>())
                .ReturnsForAnyArgs(info =>
                {
                    var validationContext = info.Arg<ValidationContext<CreditCard>>();
                    validationContext.AddFailure("CreditCard", "issue with shopper's credit card");
                    return new ValidationResult();
                });
                
            //Act
            var validationResult = _systemUnderTest.Validate(invalidCommand);
            
            //Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Test]
        public void Should_NotBeValid_When_AmountInvalid()
        {
            //Arrange
            var amount = new Amount();
            var invalidCommand = new CreatePaymentCommand
            {
                Amount = amount
            };
            _paymentAmountValidator.Validate(Arg.Any<IValidationContext>())
                .ReturnsForAnyArgs(info =>
                {
                    var validationContext = info.Arg<ValidationContext<Amount>>();
                    validationContext.AddFailure("Amount", "amount is wrong");
                    return new ValidationResult();
                });
                

            //Act
            var validationResult = _systemUnderTest.Validate(invalidCommand);

            //Assert
            validationResult.IsValid.Should().BeFalse();
        }
    }
}