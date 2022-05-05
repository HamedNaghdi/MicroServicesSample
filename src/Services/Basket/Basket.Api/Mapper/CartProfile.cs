using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.Events;

namespace Basket.Api.Mapper;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartCheckout, CartCheckoutEvent>().ReverseMap();
    }
}
