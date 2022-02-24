using System;
using Application.Services;
using Domain;
using FluentValidation;

namespace Application.Validators
{
    public class CreditCardExpiryValidator : AbstractValidator<CreditCardExpiry>
    {
        public CreditCardExpiryValidator(IDateService dateService)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(expiry => expiry.Month).InclusiveBetween(1, 12);
            RuleFor(expiry => expiry.Year).InclusiveBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year);
            RuleFor(expiry => expiry).Custom((expiry, context) =>
            {
                var expiryDate = new DateTime(expiry.Year, expiry.Month, DateTime.DaysInMonth(expiry.Year, expiry.Month));
                var valid = dateService.Now.CompareTo(expiryDate) <= 0;
                if (!valid)
                {
                    context.AddFailure("Card expired");
                }
            });
        }
    }
}