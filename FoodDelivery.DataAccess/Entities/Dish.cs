using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.DataAccess.Entities
{
    public class Dish
    {
        [Key]
        public Guid Id { get; set; }                   

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool IsVegetarian { get; set; }

        public Guid CategoryId { get; set; }
        public DishCategory? Category { get; set; }

    }
}
