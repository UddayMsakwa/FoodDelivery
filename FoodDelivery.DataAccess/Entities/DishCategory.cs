using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DataAccess.Entities
{
    public class DishCategory
    {
        [Key]
        public Guid Id { get; set; }        

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
