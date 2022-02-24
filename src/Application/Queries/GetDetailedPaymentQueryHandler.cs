using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Queries
{
    internal class GetDetailedPaymentQueryHandler : IRequestHandler<GetDetailedPaymentQuery, DetailedPayment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public GetDetailedPaymentQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<DetailedPayment> Handle(GetDetailedPaymentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetOrThrowAsync(request.PaymentId);

            var detailedPayment = _mapper.Map<DetailedPayment>(payment);

            return detailedPayment;

        }
    }
}