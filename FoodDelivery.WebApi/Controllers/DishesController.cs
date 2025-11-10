using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // ===========================
        // GET /api/dishes
        // ===========================
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string[]? categories,
            [FromQuery] bool? vegetarianOnly,
            [FromQuery] int? sortBy,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _dishService.GetAllAsync(categories, vegetarianOnly, sortBy, page, pageSize);
            return Ok(new
            {
                total = result.Total,
                items = result.Items
            });
        }

        // ===========================
        // GET /api/dishes/{id}
        // ===========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
                return NotFound(new { message = "Dish not found." });

            return Ok(dish);
        }

        // ===========================
        // POST /api/dishes
        // ===========================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDishDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _dishService.CreateAsync(dto);
            return Ok(created);
        }

        // ===========================
        // PUT /api/dishes/{id}
        // ===========================
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateDishDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _dishService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        // ===========================
        // DELETE /api/dishes/{id}
        // ===========================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dishService.DeleteAsync(id);
            return NoContent();
        }
    }
}
