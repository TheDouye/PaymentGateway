using System;

namespace Domain.Exceptions
{
    public class BankProcessingException : Exception
    {
        private readonly Payment _paymentId;
        
        public BankProcessingException(Payment paymentId)
        {
            _paymentId = paymentId;
        }
        public string PaymentId => _paymentId.Id;
    }
}