using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class CreditCardNumberValidator : AbstractValidator<CreditCardNumber>
    {
        public CreditCardNumberValidator()
        {
            RuleFor(number => number.Value).Matches("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$").OverridePropertyName("CreditCardNumber");
        }
    }
}