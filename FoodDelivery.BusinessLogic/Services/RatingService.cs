using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dto.DishId);

            if (existing != null)
                throw new InvalidOperationException("User has already rated this dish.");

            var hasOrdered = await _context.OrderItems
                .AnyAsync(oi => oi.DishId == dto.DishId && oi.Order.UserId == userId);

            if (!hasOrdered)
                throw new InvalidOperationException("User can only rate dishes they have ordered.");

            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                DishId = dto.DishId,
                UserId = userId,
                Score = dto.Score,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return new RatingResponseDto
            {
                Id = rating.Id,
                DishId = rating.DishId,
                UserId = rating.UserId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt
            };
        }

        public async Task<IEnumerable<RatingResponseDto>> GetDishRatingsAsync(Guid dishId)
        {
            return await _context.Ratings
                .Where(r => r.DishId == dishId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RatingResponseDto
                {
                    Id = r.Id,
                    DishId = r.DishId,
                    UserId = r.UserId,
                    Score = r.Score,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(Guid dishId)
        {
            return await _context.Ratings
                .Where(r => r.DishId == dishId)
                .AverageAsync(r => (double?)r.Score) ?? 0.0;
        }
    }
}
