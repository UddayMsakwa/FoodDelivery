using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _ratingService.AddRatingAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{dishId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDishRatings(Guid dishId)
        {
            var ratings = await _ratingService.GetDishRatingsAsync(dishId);
            return Ok(ratings);
        }

        [HttpGet("{dishId}/average")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(Guid dishId)
        {
            var avg = await _ratingService.GetAverageRatingAsync(dishId);
            return Ok(new { DishId = dishId, Average = avg });
        }
    }
}
