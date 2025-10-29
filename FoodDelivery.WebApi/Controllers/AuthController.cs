using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _svc;
        public AuthController(IAuthService s) { _svc = s; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest r)
        {
            try { return Ok(await _svc.RegisterAsync(r)); }
            catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest r)
        {
            try { return Ok(await _svc.LoginAsync(r)); }
            catch (Exception ex) { return Unauthorized(new { error = ex.Message }); }
        }
    }
}
