using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Infrastructure.Banks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Merchant repository mock. In Infrastructure as it would probably be a remote service or a repo fed from a service
    /// </summary>
    
    [ExcludeFromCodeCoverage(Justification = "Out of the scope. Stub to provide all onboarded merchants")]
    public class MerchantRepositoryMock : IMerchantRepository
    {
        private readonly ConcurrentDictionary<string, IMerchant> _merchants = new();

        public MerchantRepositoryMock()
        {
            var paymentRepositoryMock = new PaymentRepositoryMock();

            AddMerchant("Apple", "7da98d82-5f77-46d9-9531-5472779e1f97", new MorganStanley(), paymentRepositoryMock);
            
            AddMerchant("Abble", "ffe03beb-d2fe-43f2-a10e-fe7ee47daba9", new CreditMutuelDuCoin(), paymentRepositoryMock);
        }

        private void AddMerchant(string name, string id, IBank bank, IPaymentRepository paymentRepositoryMock)
        {
            var merchant = new Merchant(id, name, paymentRepositoryMock, bank, new PaymentIdGenerator());
            
            _merchants.TryAdd(id, merchant);
        }

        public Task<bool> TryGetMerchantByIdAsync(string id, out IMerchant merchant)
        {
            merchant = null;

            var tryGetMerchantAsync = !string.IsNullOrEmpty(id) && _merchants.TryGetValue(id, out merchant);

            return Task.FromResult(tryGetMerchantAsync);
        }

        public Task<IMerchant> GetMerchantByNameOrDefaultAsync(string name)
        {
            var keyValuePair = _merchants.FirstOrDefault(pair => pair.Value.Name == name);

            return Task.FromResult(keyValuePair.Value);
        }
        
        public Task<IMerchant> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !_merchants.TryGetValue(id, out var merchant))
            {
                throw new MerchantNotRegisteredException(id);
            }

            return Task.FromResult(merchant);
        }
    }
}