using System;
using Application.Commands;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidator<CreatePaymentCommand>, CreatePaymentCommandValidator>(CreateCreatePaymentCommandValidator);
            serviceCollection.AddTransient<CreditCardValidator>();
            serviceCollection.AddTransient<IValidator<CreditCardExpiry>, CreditCardExpiryValidator>();
            serviceCollection.AddTransient<IValidator<CreditCardNumber>, CreditCardNumberValidator>();
            serviceCollection.AddTransient<IValidator<CreditCardCvv>, CreditCardCvvValidator>();
            serviceCollection.AddTransient<PaymentAmountValidator>();
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return serviceCollection;
        }

        private static CreatePaymentCommandValidator CreateCreatePaymentCommandValidator(IServiceProvider provider)
        {
            return new CreatePaymentCommandValidator(provider.GetRequiredService<CreditCardValidator>(),
                provider.GetRequiredService<PaymentAmountValidator>());
        }
    }
}