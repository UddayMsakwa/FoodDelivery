using FoodDelivery.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IRatingService
    {
        Task<RatingResponseDto> AddRatingAsync(Guid userId, RatingDto dto);
        Task<IEnumerable<RatingResponseDto>> GetDishRatingsAsync(Guid dishId);
        Task<double> GetAverageRatingAsync(Guid dishId);
    }
}
