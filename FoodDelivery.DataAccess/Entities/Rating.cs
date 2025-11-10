using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DataAccess.Entities
{
    public class Rating
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid DishId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Dish Dish { get; set; } = null!;
    }
}
