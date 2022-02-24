using System;

namespace Domain
{
    public class PaymentIdGenerator : IPaymentIdGenerator
    {
        public string GeneratePaymentId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}