using System;
using System.Collections.Generic;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
