namespace FoodDelivery.BusinessLogic.DTOs
{
    public class RatingResponseDto
    {
        public Guid Id { get; set; }
        public string DishName { get; set; } = null!;
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
