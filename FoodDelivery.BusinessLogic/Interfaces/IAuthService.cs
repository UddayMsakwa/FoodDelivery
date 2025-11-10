using FoodDelivery.DataAccess.Entities;
using FoodDelivery.BusinessLogic.DTOs.Auth;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}
