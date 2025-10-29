using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext ctx) { _context = ctx; }

        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            var u = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
                    ?? throw new Exception("User not found");
            return new UserProfileDto
            {
                Email = u.Email,
                Name = u.Name,
                BirthDate = u.BirthDate,
                Address = u.Address,
                Phone = u.Phone
            };
        }

        public async Task UpdateProfileAsync(Guid userId, UpdateProfileRequest r)
        {
            var u = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
                    ?? throw new Exception("User not found");
            if (r.Phone != null)
            {
                var p = @"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}-\d{2}$";
                if (!Regex.IsMatch(r.Phone, p))
                    throw new Exception("Invalid phone format (+7 (xxx) xxx-xx-xx-xx)");
            }
            u.Name = r.Name; u.BirthDate = r.BirthDate; u.Address = r.Address; u.Phone = r.Phone;
            await _context.SaveChangesAsync();
        }
    }
}
