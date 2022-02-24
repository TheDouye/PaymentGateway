using System;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain
{
    public class Merchant : IMerchant,  IAggregateRoot
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBank _bank;
        private readonly IPaymentIdGenerator _paymentIdGenerator;
        public Merchant(string id, string name, IPaymentRepository paymentRepository, IBank bank, IPaymentIdGenerator paymentIdGenerator)
        {
            Id = id;
            Name = name;
            _paymentRepository = paymentRepository;
            _bank = bank;
            _paymentIdGenerator = paymentIdGenerator;
        }
        
        public string Name { get; }

        public string Id { get; }
        
        public async Task<Payment> CashInNewPayment(CreditCard creditCard, Amount amount)
        {
            var pendingPayment = new Payment(_paymentIdGenerator.GeneratePaymentId(), creditCard, amount);

            try
            {
                return await ProcessPayment(creditCard, amount, pendingPayment);
            }
            
            //Very very bad: I do not have enough time but I should re-work this exception handling:
            // We should log at least
            // Maybe it would be more appropriate to use some monadic class like https://louthy.github.io/language-ext/LanguageExt.Core/Monads/Alternative%20Value%20Monads/Either/Either/index.html
            
            catch (BankTimedOutException e)
            {
                pendingPayment.Fail(e.Reason);
            }
            catch (Exception)
            {
                pendingPayment.Fail("Bank processing failed for unknown reason");
            }
            
            await SavePayment(pendingPayment);

            return pendingPayment;
        }

        private async Task<Payment> ProcessPayment(CreditCard creditCard, Amount amount, Payment pendingPayment)
        {
            var bankPaymentResult = await _bank.ProcessPaymentAsync(creditCard, amount);

            UpdatePaymentStatus(bankPaymentResult.Status, pendingPayment);

            UpdateBankReference(pendingPayment, bankPaymentResult);

            await SavePayment(pendingPayment);

            return pendingPayment;
        }

        private async Task SavePayment(Payment pendingPayment)
        {
            await _paymentRepository.AddAsync(pendingPayment);
        }

        private static void UpdateBankReference(Payment pendingPayment, BankPaymentResult bankPaymentResult)
        {
            pendingPayment.UpdateBankPaymentResult(bankPaymentResult);
        }

        //Simplistic
        private static void UpdatePaymentStatus(BankPaymentStatus status, Payment pendingPayment)
        {
            switch (status)
            {
                case BankPaymentStatus.Rejected:
                    pendingPayment.Reject();
                    break;
                case BankPaymentStatus.Accepted:
                    pendingPayment.Accept();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}