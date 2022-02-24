using Domain;
using MediatR;

namespace Application.Commands
{
    public class CreatePaymentCommand : IRequest<CreatePaymentResult>
    {
        public string MerchantId { get; init; }

        //To be discussed: DOMAIN Value objects (only) on Command ?
        // Depends on teams.
        public CreditCard CreditCard { get; init; }
        public Amount Amount { get; init; }
    }
}