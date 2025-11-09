namespace FoodDelivery.BusinessLogic.DTOs
{
    public class CartItemDto
    {
        public Guid DishId { get; set; }
        public string DishName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
