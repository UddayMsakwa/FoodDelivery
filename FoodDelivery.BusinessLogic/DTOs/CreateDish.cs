using System;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class CreateDishDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        public Guid DishCategoryId { get; set; }
    }
}
