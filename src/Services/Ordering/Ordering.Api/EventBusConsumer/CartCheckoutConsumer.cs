using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.Api.EventBusConsumer;

public class CartCheckoutConsumer : IConsumer<CartCheckoutEvent>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CartCheckoutConsumer> _logger;

    public CartCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<CartCheckoutConsumer> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<CartCheckoutEvent> context)
    {
        var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        var newOrderId = await _mediator.Send(command);
        _logger.LogInformation($"CartCheckoutEvent consumed successfully. Created Order Id : {newOrderId}");
    }
}
