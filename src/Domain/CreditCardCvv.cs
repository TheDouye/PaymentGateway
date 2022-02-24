namespace Domain
{
    public struct CreditCardCvv
    {
        public string Code { get; }

        private CreditCardCvv(string code)
        {
            Code = code;
        }

        public static CreditCardCvv Create(string visibleCvv)
        {
            return new CreditCardCvv(visibleCvv);
        }
    }
}