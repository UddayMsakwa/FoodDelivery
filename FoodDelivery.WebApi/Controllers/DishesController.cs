using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess.Entities;

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string[] categories,
                                                [FromQuery] bool? vegetarianOnly,
                                                [FromQuery] int? sortBy,
                                                [FromQuery] int page = 1,
                                                [FromQuery] int pageSize = 10)
        {
            var dishes = await _dishService.GetAllAsync(categories, vegetarianOnly, sortBy, page, pageSize);
            return Ok(new { total = dishes.Count(), items = dishes });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null) return NotFound();
            return Ok(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dish dish)
        {
            var result = await _dishService.CreateAsync(dish);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Dish dish)
        {
            var result = await _dishService.UpdateAsync(id, dish);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _dishService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
