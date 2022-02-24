namespace PaymentGateway.Api.Dtos
{
    public class CreatePayment 
    {
        public string MerchantId { get; set; }
        public string CreditCardNumber { get; set; }
        public int CreditCardExpiryMonth { get; set; }
        public int CreditCardExpiryYear { get; set; }
        public string CreditCardCvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}