using System;
using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    }
}
