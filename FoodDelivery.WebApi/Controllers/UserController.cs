using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;

namespace FoodDelivery.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService s) { _svc = s; }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpGet("me")]
        public async Task<IActionResult> Me() => Ok(await _svc.GetProfileAsync(GetUserId()));

        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UpdateProfileRequest r)
        { await _svc.UpdateProfileAsync(GetUserId(), r); return Ok(new { message = "Profile updated" }); }
    }
}
