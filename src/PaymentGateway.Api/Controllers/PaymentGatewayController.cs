using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Dtos;
using PaymentGateway.Api.Exceptions;

namespace PaymentGateway.Api.Controllers
{
    /// <summary>
    /// Interact with the payment gateway by submitting a payment or getting detailed payment information
    /// </summary>
    [Produces("application/json")]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    [Route("api/payments")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Main ctor
        /// </summary>
        /// <param name="mediator">Send command and query</param>
        /// <param name="mapper">Map dto to command an query</param>
        public PaymentGatewayController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Request a payment to the gateway
        /// </summary>
        /// <param name="payment">Command to request a payment</param>
        /// <returns>The payment id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/payments
        ///     {
        ///       "merchantId": "7da98d82-5f77-46d9-9531-5472779e1f97"
        ///       "cardNumber": "1111 2222 3333 4444",
        ///       "cardExpiryMonth": 03,
        ///       "cardExpiryYear": 2069,
        ///       "cardCvv": 789,
        ///       "amount": 1500,
        ///       "currency": "EUR"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created payment id</response>
        /// <response code="422">Returns the validation errors</response> 
        [HttpPost, ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<string>> ProcessPayment([FromBody] CreatePayment payment)
        {
            var createPaymentCommand = _mapper.Map<CreatePaymentCommand>(payment);

            var createPaymentResult = await _mediator.Send(createPaymentCommand);
            
            return CreatedAtAction("processPayment", createPaymentResult.PaymentId);
        }

        /// <summary>
        /// Retrieve a payment detail via a payment id
        /// </summary>
        /// <param name="paymentId">Id of the payment</param>
        /// <returns>Payment with detailed information</returns>
        [HttpGet("{paymentId}"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetailedPayment>> GetPayment(string paymentId)
        {
            var detailedPayment = await _mediator.Send(new GetDetailedPaymentQuery(paymentId));
            
            return Ok(detailedPayment);
        }
    }
}