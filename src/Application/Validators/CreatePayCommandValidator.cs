using Application.Commands;
using Domain;
using FluentValidation;

namespace Application.Validators
{
    internal class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator(IValidator<CreditCard> creditCardValidator,
            IValidator<Amount> amountValidator)
        {
            RuleFor(command => command.CreditCard).SetValidator(creditCardValidator);
            RuleFor(command => command.Amount).SetValidator(amountValidator);
        }
    }
}