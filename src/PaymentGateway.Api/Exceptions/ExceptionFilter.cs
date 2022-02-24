using System.Linq;
using System.Net;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Api.Exceptions
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException validationException:
                    context.Result =
                        new ObjectResult(
                            $"Validation failed: {string.Join(",", validationException.Errors.Select(failure => failure.ErrorMessage).Where(error => !string.IsNullOrEmpty(error)))}")
                        {
                            StatusCode = 422
                        };
                    _logger.LogError(validationException, "Validation errors: {@}", validationException.Errors);
                    break;

                case PaymentNotFoundException paymentNotFoundException:
                    context.Result = new StatusCodeResult(404);
                    _logger.LogError(paymentNotFoundException, "Can not find payment id {paymentId}", paymentNotFoundException.PaymentId);
                    break;

                case MerchantNotRegisteredException merchantNotRegisteredException:
                    context.Result = new StatusCodeResult(401);
                    _logger.LogError(merchantNotRegisteredException, "Merchant {id}", merchantNotRegisteredException.Id);
                    break;

                default:
                    context.Result = context.Result;
                    break;
            }
        }
    }
}