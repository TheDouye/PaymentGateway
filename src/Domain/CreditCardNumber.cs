using System.Text;

namespace Domain
{
    public readonly struct CreditCardNumber
    {
        public string Value { get; }

        private CreditCardNumber(string value)
        {
            Value = value;
        }

        public static CreditCardNumber Create(string detailedNumber)
        {
            return new CreditCardNumber(detailedNumber);
        }

        public string Mask()
        {
            //Yes... I know.
            var maskNumber = new StringBuilder(Value)
            {
                [5] = 'X',
                [6] = 'X',
                [7] = 'X',
                [8] = 'X',
                [10] = 'X',
                [11] = 'X',
                [12] = 'X',
                [13] = 'X'
            }.ToString();

            return maskNumber;
        }
    }
}