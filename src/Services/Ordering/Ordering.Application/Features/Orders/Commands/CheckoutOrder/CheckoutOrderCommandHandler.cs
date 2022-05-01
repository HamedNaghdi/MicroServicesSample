using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    #region Fields

    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    #endregion

    #region Ctor

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
        IMapper mapper,
        IEmailService emailService,
        ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region Utilities

    private async Task SendMail(Order order)
    {
        var email = new Email() { To = "hamed.naghdi@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

        try
        {
            await _emailService.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
        }
    }

    #endregion

    #region Methods

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);
        var insertedOrder = await _orderRepository.AddAsync(order);

        _logger.LogInformation($"Order {insertedOrder.Id} is successfully created.");

        await SendMail(insertedOrder);

        return insertedOrder.Id;
    }

    #endregion
}
