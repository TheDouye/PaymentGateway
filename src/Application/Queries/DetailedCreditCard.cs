namespace Application.Queries
{
    public class DetailedCreditCard
    {
        public string Number { get; init; }
        public DetailedCreditCardExpiry Expiry { get; init; }
        public string Cvv { get; init; }
    }
}