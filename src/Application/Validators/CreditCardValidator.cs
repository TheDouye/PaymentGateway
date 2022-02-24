using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class CreditCardValidator : AbstractValidator<CreditCard>
    {
        public CreditCardValidator(IValidator<CreditCardExpiry> expiryValidator, IValidator<CreditCardNumber> cardNumberValidator, IValidator<CreditCardCvv> cvvValidator)
        {
            RuleFor(card => card.Expiry).SetValidator(expiryValidator).OverridePropertyName("CreditCardExpiry");
            RuleFor(card => card.Number).SetValidator(cardNumberValidator).OverridePropertyName("CreditCardNumber");
            RuleFor(card => card.Cvv).SetValidator(cvvValidator).OverridePropertyName("CreditCardCvv");
        }
    }
}