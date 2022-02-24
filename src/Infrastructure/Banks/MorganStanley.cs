using System;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Banks
{
    public class MorganStanley : BankBase
    {
        public MorganStanley() : base(Guid.NewGuid().ToString())
        {
        }

        public override Task<BankPaymentResult> ProcessPaymentAsync(CreditCard creditCard, Amount amount)
        {
            return Task.FromResult(new BankPaymentResult(Guid.NewGuid().ToString(), BankPaymentStatus.Accepted));
        }
    }
}