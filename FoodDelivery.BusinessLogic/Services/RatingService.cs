using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.BusinessLogic.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RatingResponseDto> AddRatingAsync(Guid userId, RatingDto dto)
        {
            // Check if user ordered this dish
            bool ordered = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.Order.UserId == userId && oi.DishId == dto.DishId);

            if (!ordered)
                throw new Exception("User cannot rate a dish they haven't ordered.");

            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DishId = dto.DishId,
                Score = dto.Score,
                Comment = dto.Comment
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            var dish = await _context.Dishes.FindAsync(dto.DishId);

            return new RatingResponseDto
            {
                Id = rating.Id,
                DishName = dish?.Name ?? "Unknown",
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt
            };
        }

        public async Task<IEnumerable<RatingResponseDto>> GetDishRatingsAsync(Guid dishId)
        {
            return await _context.Ratings
                .Include(r => r.Dish)
                .Where(r => r.DishId == dishId)
                .Select(r => new RatingResponseDto
                {
                    Id = r.Id,
                    DishName = r.Dish.Name,
                    Score = r.Score,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(Guid dishId)
        {
            if (!await _context.Ratings.AnyAsync(r => r.DishId == dishId))
                return 0;

            return await _context.Ratings
                .Where(r => r.DishId == dishId)
                .AverageAsync(r => (double)r.Score);
        }
    }
}
