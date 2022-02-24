namespace Domain
{
    public class Payment : IEntity
    {
        public Payment(string paymentId, CreditCard creditCreditCard, Amount amount)
        {
            Id = paymentId;
            CreditCard = creditCreditCard;
            Amount = amount;
            Status = PaymentStatus.Pending;
        }
        
        
        public string Id { get; }

        public BankPaymentResult BankPaymentResult { get; private set; }

        public PaymentStatus Status { get; private set; }
        
        public CreditCard CreditCard { get; }

        public Amount Amount { get; }
        public string Error { get; private set; }

        public void Reject()
        {
            Status = PaymentStatus.Rejected;
        }
        public void Accept()
        {
            Status = PaymentStatus.Accepted;
        }
        
        public void Fail(string errorMessage)
        {
            Status = PaymentStatus.Failed;
            Error = errorMessage;
        }
        
        public void UpdateBankPaymentResult(BankPaymentResult bankPaymentResult)
        {
            BankPaymentResult = bankPaymentResult;
        }
    }
}