namespace FoodDelivery.BusinessLogic.DTOs
{
    public class RatingDto
    {
        public Guid DishId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}
