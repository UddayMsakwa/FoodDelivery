using System;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class RatingResponseDto
    {
        public Guid Id { get; set; }
        public Guid DishId { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
