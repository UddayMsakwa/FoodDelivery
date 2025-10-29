using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "OK" });
    }
}
