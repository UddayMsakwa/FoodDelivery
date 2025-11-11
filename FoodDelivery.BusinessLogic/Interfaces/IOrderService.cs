using FoodDelivery.BusinessLogic.DTOs.Orders;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(Guid userId, CreateOrderDto dto);
        Task<OrderDto?> GetByIdAsync(Guid userId, Guid orderId);
        Task<(IEnumerable<OrderDto> Items, int Total)> GetMineAsync(Guid userId, int page, int pageSize);
        Task<OrderDto> UpdateStatusAsync(Guid orderId, UpdateOrderStatusDto dto); // optional (admin)
    }
}
