using System;
using System.Collections.Generic;
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
    internal class CreditCardExpiryValidatorTest
    {
        private IDateService _dateService;

        private CreditCardExpiryValidator _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _dateService = Substitute.For<IDateService>();

            _systemUnderTest = new CreditCardExpiryValidator(_dateService);
        }

        [Test]
        public async Task Should_BeValid_When_ExpiryIsAfterToday()
        {
            //Arrange
            _dateService.Now.Returns(new DateTime(2022, 3, 13));

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardExpiry.Create(11, 2069));

            //Assert
            validationResult.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Should_BeValid_When_ExpiryLastDayOfMonth()
        {
            //Arrange
            _dateService.Now.Returns(new DateTime(2022, 3, 31));

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardExpiry.Create(3, 2022));

            //Assert
            validationResult.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Should_BeInValid_When_ExpiryBeforeToday()
        {
            //Arrange
            _dateService.Now.Returns(new DateTime(2023, 3, 13));

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardExpiry.Create(3, 2022));

            //Assert
            ShouldContainSingleError(validationResult, "Card expired");
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(13)]
        public async Task Should_BeInValid_When_ExpiryMonthInvalid(int month)
        {
            //Arrange

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardExpiry.Create(month, 2069));

            //Assert
            ShouldContainSingleError(validationResult, $"'Month' must be between 1 and 12. You entered {month}."); 
        }

        [TestCaseSource(nameof(InvalidExpiryYearTestCaseSource))]
        public async Task Should_BeInValid_When_ExpiryYearInvalid(int year)
        {
            //Arrange

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardExpiry.Create(3, year));

            //Assert
            ShouldContainSingleError(validationResult, $"'Year' must be between 1 and 9999. You entered {year}.");
        }

        private static IEnumerable<TestCaseData> InvalidExpiryYearTestCaseSource
        {
            get
            {
                yield return new TestCaseData(DateTime.MinValue.Year - 1);
                yield return new TestCaseData(DateTime.MaxValue.Year + 1);
            }
        }

        private static void ShouldContainSingleError(ValidationResult validationResult, string errorMessage)
        {
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorMessage.Should().Be(errorMessage);
        }
    }
}