using FoodDelivery.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.BusinessLogic.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
