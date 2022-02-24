namespace Domain
{
    public class BankPaymentResult : IEntity
    {
        public BankPaymentResult(string id, BankPaymentStatus status = default)
        {
            Id = id;
            Status = status;
        }

        public string Id { get; }

        public BankPaymentStatus Status { get; }
    }
}