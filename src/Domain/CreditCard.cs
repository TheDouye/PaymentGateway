namespace Domain
{
    public readonly struct CreditCard
    {
        //Holder name (not in the prerequisite) ?

        public CreditCardNumber Number { get; }
        public CreditCardCvv Cvv { get; }
        public CreditCardExpiry Expiry { get; }

        private CreditCard(CreditCardNumber number, CreditCardCvv cvv, CreditCardExpiry expiry)
        {
            Number = number;
            Cvv = cvv;
            Expiry = expiry;
        }

        public static CreditCard CreateFrom(string number, int expiryMonth, int expiryYear, string cvv)
        {
            return new CreditCard(CreditCardNumber.Create(number), CreditCardCvv.Create(cvv), CreditCardExpiry.Create(expiryMonth, expiryYear));
        }

        public static CreditCard CreateFrom(CreditCardNumber cardNumber, CreditCardExpiry expiry, CreditCardCvv cvv)
        {
            return new CreditCard(cardNumber, cvv, expiry);
        }

    }
}