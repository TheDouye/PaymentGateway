using System;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;

namespace Infrastructure.Repositories
{
    public class CreditMutuelDuCoin : BankBase
    {
        public CreditMutuelDuCoin() : base(Guid.NewGuid().ToString())
        {
        }

        public override Task<BankPaymentResult> ProcessPaymentAsync(CreditCard creditCard, Amount amount)
        {
            throw new BankTimedOutException(Id, "Network issue");
        }
    }
}