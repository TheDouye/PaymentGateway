using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
namespace Application.Commands
{
    internal class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResult>
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly ILogger<CreatePaymentCommandHandler> _logger;

        public CreatePaymentCommandHandler(IMerchantRepository merchantRepository, ILogger<CreatePaymentCommandHandler> logger)
        {
            _merchantRepository = merchantRepository;
            _logger = logger;
        }

        public async Task<CreatePaymentResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request received: {@command}", request);

            var merchant = await _merchantRepository.GetMerchantByIdAsync(request.MerchantId);

            var initiatedPayment = await merchant.CashInNewPayment(request.CreditCard, request.Amount);

            return new CreatePaymentResult { PaymentId = initiatedPayment.Id };
        }

    }
}