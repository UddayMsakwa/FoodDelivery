namespace FoodDelivery.DataAccess.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }

        public int CategoryId { get; set; }
        public DishCategory Category { get; set; } = null!;
    }
}
