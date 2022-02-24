using Application.Commands;
using Application.Queries;
using AutoMapper;
using Domain;
using PaymentGateway.Api.Dtos;

namespace PaymentGateway.Api.Mapping
{
    internal class GatewayProfile : Profile
    {
        public GatewayProfile()
        {
            CreateMap<CreatePayment, CreatePaymentCommand>()
                .ForMember(command => command.Amount, expression => expression.MapFrom(dto => Amount.Create(dto.Amount, dto.Currency)))
                .ForMember(command => command.CreditCard, expression => expression.MapFrom(dto => CreditCard.CreateFrom(dto.CreditCardNumber, dto.CreditCardExpiryMonth, dto.CreditCardExpiryYear, dto.CreditCardCvv)));

            CreateMap<Payment, DetailedPayment>();
            CreateMap<CreditCard, DetailedCreditCard>()
                .ForMember(card => card.Number, expression => expression.MapFrom(card => card.Number.Mask()))
                .ForMember(card => card.Cvv, expression => expression.MapFrom(card => card.Cvv.Code));
            CreateMap<CreditCardExpiry, DetailedCreditCardExpiry>();

        }
    }
}