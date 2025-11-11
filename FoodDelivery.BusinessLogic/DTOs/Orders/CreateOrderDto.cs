using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.BusinessLogic.DTOs.Orders
{
    public class CreateOrderDto
    {
        [MinLength(1)]
        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
