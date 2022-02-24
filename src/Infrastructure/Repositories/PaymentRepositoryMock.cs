using System.Collections.Concurrent;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;

namespace Infrastructure.Repositories
{
    public class PaymentRepositoryMock : IPaymentRepository
    {
        private static readonly ConcurrentDictionary<string, Payment> Payments = new();

        public Task AddAsync(Payment newPayment)
        {
            Payments.TryAdd(newPayment.Id, newPayment);

            return Task.CompletedTask;
        }

        public Task<Payment> GetOrThrowAsync(string paymentId)
        {
            var tryGetValue = Payments.TryGetValue(paymentId, out var payment);
            if (tryGetValue)
            {
                return Task.FromResult(payment);
            }

            throw new PaymentNotFoundException(paymentId);
        }
    }
}