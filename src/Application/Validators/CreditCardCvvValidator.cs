using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class CreditCardCvvValidator : AbstractValidator<CreditCardCvv>
    {
        public CreditCardCvvValidator()
        {
            RuleFor(cvv => cvv.Code).Matches("^[0-9]{3}$");
        }
    }
}