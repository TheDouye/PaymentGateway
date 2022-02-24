using System.Threading.Tasks;

namespace Domain
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment newPayment);
        Task<Payment> GetOrThrowAsync(string paymentId);
    }
}