using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Application.Services;

namespace Infrastructure.Services
{
    [ExcludeFromCodeCoverage(Justification = "Out of the scope. Mock to provide all ISO currencies")]
    public class CurrencyServiceMock : ICurrencyService
    {
        public Task<bool> IsIsoCurrency(string currencyName)
        {
            return Task.FromResult(currencyName == "EUR" || currencyName == "USD");
        }
        
    }
}