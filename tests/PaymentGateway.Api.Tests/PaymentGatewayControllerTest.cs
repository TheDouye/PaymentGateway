using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Dtos;

namespace PaymentGateway.Api.Tests
{
    public class PaymentGatewayControllerTest
    {
        private IMediator _mediator;
        private IMapper _mapper;

        private PaymentGatewayController _systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _mapper = Substitute.For<IMapper>();
            
            _systemUnderTest = new PaymentGatewayController(_mediator, _mapper);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnStatusCode201()
        {
            //Arrange
            ReturnPaymentResultForAnyPaymentCommand();
            
            //Act
            var createdActionResult = await ProcessPaymentAsync(CreatePaymentDto());

            //Assert
            createdActionResult.StatusCode.Should().Be(201);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnPaymentResultId()
        {
            //Arrange
            var createPaymentDto = CreatePaymentDto();
            var createPaymentCommand = CreatePaymentCommand();
            const string expectedPaymentId = "payment#87";
            MockResultForCommand(createPaymentCommand, expectedPaymentId);
            MapDtoToCommand(createPaymentDto, createPaymentCommand);
            
            //Act
            var createdActionResult = await ProcessPaymentAsync(createPaymentDto);

            //Assert
            createdActionResult.Value.Should().Be(expectedPaymentId);
        }

        [Test]
        public async Task GetPayment_Should_ReturnDetailedPayment()
        {
            //Arrange
            const string paymentId = "payment#254";
            var expectedDetailedPayment = new DetailedPayment();
            MockResultForQuery(paymentId, expectedDetailedPayment);
            
            //Act
            var okResult = await _systemUnderTest.GetPayment(paymentId);

            //Assert
            okResult.Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .Be(expectedDetailedPayment);

        }

        [Test]
        public async Task ProcessPayment_Should_ReturnActionName()
        {
            //Arrange
            ReturnPaymentResultForAnyPaymentCommand();

            //Act
            var createdActionResult = await ProcessPaymentAsync(CreatePaymentDto());

            //Assert
            createdActionResult.ActionName.Should().Be("processPayment");
        }

        private void MockResultForQuery(string paymentId, DetailedPayment expectedDetailedPayment)
        {
            _mediator
                .Send(Arg.Is<GetDetailedPaymentQuery>(query => query.PaymentId == paymentId))
                .Returns(Task.FromResult(expectedDetailedPayment));
        }

        private void MapDtoToCommand(CreatePayment createPayment, CreatePaymentCommand createPaymentCommand)
        {
            _mapper.Map<CreatePaymentCommand>(createPayment).Returns(createPaymentCommand);
        }

        private void ReturnPaymentResultForAnyPaymentCommand()
        {
            _mediator.Send(Arg.Any<CreatePaymentCommand>()).Returns(new CreatePaymentResult(){PaymentId = "payment#any"});
        }


        private static CreatePaymentCommand CreatePaymentCommand()
        {
            return new CreatePaymentCommand();
        }
        
        private static CreatePayment CreatePaymentDto()
        {
            return new CreatePayment();
        }

        private async Task<CreatedAtActionResult> ProcessPaymentAsync(CreatePayment createPayment)
        {
            var actualPaymentResult = await _systemUnderTest.ProcessPayment(createPayment);

            return actualPaymentResult.Result.As<CreatedAtActionResult>();
        }

        private void MockResultForCommand(CreatePaymentCommand createPaymentCommand, string gatewayPaymentId)
        {
            var createPaymentResult = new CreatePaymentResult {PaymentId = gatewayPaymentId};

            _mediator.Send(createPaymentCommand).Returns(Task.FromResult(createPaymentResult));
        }
    }
}
