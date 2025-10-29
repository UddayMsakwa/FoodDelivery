using System;
using System.Collections.Generic;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
