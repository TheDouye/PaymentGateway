using System.Threading.Tasks;

namespace Domain
{
    public interface IBank
    {
        Task<BankPaymentResult> ProcessPaymentAsync(CreditCard creditCard, Amount amount);
    }
}