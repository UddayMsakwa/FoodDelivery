using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatingAsync(Guid userId, RatingDto dto);
        Task<IEnumerable<RatingResponseDto>> GetDishRatingsAsync(Guid dishId);
        Task<double> GetAverageRatingAsync(Guid dishId);
    }
}
