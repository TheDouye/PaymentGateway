using Application.Services;
using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class PaymentAmountValidator : AbstractValidator<Amount>
    {
        public PaymentAmountValidator(ICurrencyService currencyService)
        {
            RuleFor(amount => amount.Currency).Custom((currency, context) =>
            {
                if (!currencyService.IsIsoCurrency(currency).Result)
                {
                    context.AddFailure("Currency not ISO");
                }
            });
            RuleFor(amount => amount.Value).GreaterThan(0).OverridePropertyName("Amount");
        }
    }
}