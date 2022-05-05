using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Api.EventBusConsumer;

public class CartCheckoutConsumer : IConsumer<CartCheckoutEvent>
{
    public Task Consume(ConsumeContext<CartCheckoutEvent> context)
    {
        throw new NotImplementedException();
    }
}
