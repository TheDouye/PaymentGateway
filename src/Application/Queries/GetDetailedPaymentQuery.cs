using MediatR;

namespace Application.Queries
{
    public class GetDetailedPaymentQuery : IRequest<DetailedPayment>
    {
        public string PaymentId { get; }

        public GetDetailedPaymentQuery(string paymentId)
        {
            PaymentId = paymentId;
        }
    }
}