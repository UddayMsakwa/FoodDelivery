namespace FoodDelivery.BusinessLogic.DTOs
{
    public class DishListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
        public double AverageRating { get; set; }
        public string Category { get; set; } = null!;
    }
}
