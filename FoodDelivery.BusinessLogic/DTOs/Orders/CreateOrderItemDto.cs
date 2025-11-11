using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.BusinessLogic.DTOs.Orders
{
    public class CreateOrderItemDto
    {
        [Required]
        public Guid DishId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
