using System;
using System.Collections.Generic;

namespace FoodDelivery.DataAccess.Entities
{
    public class DishCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
