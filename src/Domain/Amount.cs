namespace Domain
{
    public struct Amount
    {
        public decimal Value { get; }
        public string Currency { get; }

        private Amount(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        public static Amount Create(decimal amount, string currency)
        {
            return new Amount(amount, currency);
        }
    }
}