using Application.Services;
using Domain;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMerchantRepository, MerchantRepositoryMock>()
                .AddSingleton<ICurrencyService, CurrencyServiceMock>()
                .AddSingleton<IDateService, DateServiceMock>()
                .AddSingleton<IPaymentRepository, PaymentRepositoryMock>();
        }
    }
}