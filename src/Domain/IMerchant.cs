using System.Threading.Tasks;

namespace Domain
{
    public interface IMerchant : IEntity
    {
        string Name { get; }
        Task<Payment> CashInNewPayment(CreditCard creditCard, Amount amount);
    }
}