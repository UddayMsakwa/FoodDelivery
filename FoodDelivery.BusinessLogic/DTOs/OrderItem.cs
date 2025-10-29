namespace FoodDelivery.BusinessLogic.DTOs
{
    public class OrderItemDto
    {
        public int DishId { get; set; }
        public string DishName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
