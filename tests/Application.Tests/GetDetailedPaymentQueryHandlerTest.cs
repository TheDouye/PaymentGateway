using System.Threading;
using System.Threading.Tasks;
using Application.Queries;
using AutoMapper;
using Domain;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Application.Tests
{
    internal class GetDetailedPaymentQueryHandlerTest
    {
        private IPaymentRepository _paymentRepository;
        private IMapper _mapper;

        private GetDetailedPaymentQueryHandler _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _mapper = Substitute.For<IMapper>();
            
            _systemUnderTest = new GetDetailedPaymentQueryHandler(_paymentRepository, _mapper);
        }
        
        [Test]
        public async Task Should_GetDetailedPayment_Mapped_To_Payment()
        {
            //Arrange
            const string paymentId = "payment123";
            var payment = new Payment(paymentId, new CreditCard(), new Amount());
            MockPaymentRepositoryForQuery(paymentId, payment);
            var expectedDetailedPayment = new DetailedPayment();
            MockPaymentToDetailedPayment(payment, expectedDetailedPayment);
            
            //Act
            var actualDetailedPayment = await _systemUnderTest.Handle(new GetDetailedPaymentQuery(paymentId), CancellationToken.None);

            //Assert
            actualDetailedPayment.Should().BeEquivalentTo(expectedDetailedPayment);
        }

        private void MockPaymentToDetailedPayment(Payment payment, DetailedPayment expectedDetailedPayment)
        {
            _mapper.Map<DetailedPayment>(payment).Returns(expectedDetailedPayment);
        }

        private void MockPaymentRepositoryForQuery(string paymentId, Payment payment)
        {
            _paymentRepository.GetOrThrowAsync(paymentId).Returns(payment);
        }
    }
}