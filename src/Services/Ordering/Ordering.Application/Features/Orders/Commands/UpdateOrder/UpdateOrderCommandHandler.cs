using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var dbOrder = await _orderRepository.GetByIdAsync(request.Id);

        if (dbOrder is null)
        {
            _logger.LogError("Order not exist on database.");
            throw new DllNotFoundException(nameof(dbOrder));
        }

        _mapper.Map(request, dbOrder, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(dbOrder);

        _logger.LogInformation($"Order {dbOrder.Id} is successfully updated.");

        return Unit.Value;
    }
}
