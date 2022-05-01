﻿using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrderByUsername(string username);
}
