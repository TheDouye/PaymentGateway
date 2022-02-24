using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICurrencyService
    {
        Task<bool> IsIsoCurrency(string currencyName);
    }
}