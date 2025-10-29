using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodDelivery.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ✅ Register a new user
        public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
        {
            try
            {
                // Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == req.Email))
                    throw new Exception("User already exists");

                // Create new user entity
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = req.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                    Name = req.Name,
                    BirthDate = req.BirthDate?.ToUniversalTime(),
                    Address = req.Address,
                    Phone = req.Phone
                };


                // Add and save
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Return JWT token
                return new AuthResponse { Token = GenerateJwtToken(user) };
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Registration failed → {inner}");
            }
        }

        // ✅ Login existing user
        public async Task<AuthResponse> LoginAsync(LoginRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email)
                       ?? throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return new AuthResponse { Token = GenerateJwtToken(user) };
        }

        // ✅ JWT token generator
        private string GenerateJwtToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
