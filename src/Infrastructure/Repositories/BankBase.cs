using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Repositories
{
    public abstract class BankBase : IBank, IEntity
    {
        protected BankBase(string id)
        {
            Id = id;
        }
        public abstract Task<BankPaymentResult> ProcessPaymentAsync(CreditCard creditCard, Amount amount);
        public string Id { get; }
    }
}