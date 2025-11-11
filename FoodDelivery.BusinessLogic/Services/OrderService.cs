using FoodDelivery.BusinessLogic.DTOs.Orders;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderDto> CreateAsync(Guid userId, CreateOrderDto dto)
        {
            if (dto.Items is null || dto.Items.Count == 0)
                throw new ArgumentException("Order must contain at least one item.");

            // Fetch dishes in one roundtrip
            var dishIds = dto.Items.Select(i => i.DishId).Distinct().ToArray();
            var dishes = await _db.Dishes
                .Where(d => dishIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id);

            // Validate & create items
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            decimal total = 0m;

            foreach (var item in dto.Items)
            {
                if (!dishes.TryGetValue(item.DishId, out var dish))
                    throw new KeyNotFoundException($"Dish not found: {item.DishId}");

                if (item.Quantity < 1)
                    throw new ArgumentException("Quantity must be >= 1");

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    DishId = dish.Id,
                    Quantity = item.Quantity,
                    UnitPrice = dish.Price
                };

                total += orderItem.UnitPrice * orderItem.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalPrice = total;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Reload with joins for DTO mapping
            var saved = await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Dish)
                .FirstAsync(o => o.Id == order.Id);

            return Map(saved);
        }

        public async Task<OrderDto?> GetByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            return order is null ? null : Map(order);
        }

        public async Task<(IEnumerable<OrderDto> Items, int Total)> GetMineAsync(Guid userId, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _db.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Dish);

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data.Select(Map).ToList(), total);
        }

        public async Task<OrderDto> UpdateStatusAsync(Guid orderId, UpdateOrderStatusDto dto)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order is null) throw new KeyNotFoundException("Order not found.");
            order.Status = dto.Status;
            await _db.SaveChangesAsync();

            var saved = await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Dish)
                .FirstAsync(o => o.Id == orderId);

            return Map(saved);
        }

        private static OrderDto Map(Order o) => new()
        {
            Id = o.Id,
            UserId = o.UserId,
            OrderDate = o.OrderDate,
            TotalPrice = o.TotalPrice,
            Status = o.Status,
            Items = o.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                DishId = i.DishId,
                DishName = i.Dish?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
