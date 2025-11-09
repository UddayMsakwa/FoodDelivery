using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(Guid userId);
        Task<OrderDto?> GetOrderByIdAsync(Guid id);
        Task<bool> CancelOrderAsync(Guid id);
    }
}
