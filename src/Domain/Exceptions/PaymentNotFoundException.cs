using System;

namespace Domain.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public string PaymentId { get; }

        public PaymentNotFoundException(string paymentId)
        {
            PaymentId = paymentId;
        }
    }
}