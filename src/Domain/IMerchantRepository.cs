using System.Threading.Tasks;

namespace Domain
{
    public interface IMerchantRepository
    {
        Task<bool> TryGetMerchantByIdAsync(string id, out IMerchant merchant);
        Task<IMerchant> GetMerchantByIdAsync(string id);
        Task<IMerchant> GetMerchantByNameOrDefaultAsync(string name);
    }
}