using System;

namespace FoodDelivery.BusinessLogic.DTOs
{
    // Used in incoming order creation requests
    public class OrderItemCreateDto
    {
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
    }
}
