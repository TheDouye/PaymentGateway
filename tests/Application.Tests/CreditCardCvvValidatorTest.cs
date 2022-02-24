using System.Threading.Tasks;
using Application.Validators;
using Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Application.Tests
{
    internal class CreditCardCvvValidatorTest
    {
        private CreditCardCvvValidator _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _systemUnderTest = new CreditCardCvvValidator();
        }

        [Test]
        public async Task Should_BeValid_When_MatchesPattern()
        {
            //Arrange
            
            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardCvv.Create("098"));

            //Assert
            validationResult.Errors.Should().BeEmpty();
        }


        [Test]
        public async Task Should_BeInValid_When_DoesNotMatchPattern()
        {
            //Arrange

            //Act
            var validationResult = await _systemUnderTest.ValidateAsync(CreditCardCvv.Create("98"));

            //Assert
            validationResult.Errors.Should().Contain(failure => failure.ErrorMessage == "'Code' is not in the correct format.");

        }
    }
}