using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
