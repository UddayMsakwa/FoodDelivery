using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BusinessLogic.DTOs.Orders;
using FoodDelivery.BusinessLogic.Interfaces;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _svc;

        public OrdersController(IOrderService svc)
        {
            _svc = svc;
        }

        private Guid GetUserIdOrThrow()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue(ClaimTypes.Name) // fallback if misconfigured
                      ?? throw new UnauthorizedAccessException("User id not found in token.");
            if (!Guid.TryParse(sub, out var userId))
                throw new UnauthorizedAccessException("Invalid user id in token.");
            return userId;
        }

        // POST /api/orders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetUserIdOrThrow();
            var created = await _svc.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET /api/orders/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserIdOrThrow();
            var order = await _svc.GetByIdAsync(userId, id);
            if (order is null) return NotFound(new { message = "Order not found." });
            return Ok(order);
        }

        // GET /api/orders/my?page=1&pageSize=10
        [HttpGet("my")]
        public async Task<IActionResult> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetUserIdOrThrow();
            var result = await _svc.GetMineAsync(userId, page, pageSize);
            return Ok(new { total = result.Total, items = result.Items });
        }

        // PUT /api/orders/{id}/status   (optional admin-like route)
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // Add role checks if needed: if (!User.IsInRole("Admin")) return Forbid();
            var updated = await _svc.UpdateStatusAsync(id, dto);
            return Ok(updated);
        }
    }
}
