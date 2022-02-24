namespace Domain
{
    public struct CreditCardExpiry
    {
        public int Month { get; }
        public int Year { get; }

        private CreditCardExpiry(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public static CreditCardExpiry Create(int month, int year)
        {
            return new CreditCardExpiry(month, year);
        }
    }
}