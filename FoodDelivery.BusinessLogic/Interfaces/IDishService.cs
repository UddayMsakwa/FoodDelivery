using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IDishService
    {
        Task<(IEnumerable<DishDto> Items, int Total)> GetAllAsync(
            string[]? categories,
            bool? vegetarianOnly,
            int? sortBy,
            int page,
            int pageSize);

        Task<DishDto?> GetByIdAsync(Guid id);
        Task<DishDto> CreateAsync(CreateDishDto dto);
        Task<DishDto> UpdateAsync(Guid id, CreateDishDto dto);
        Task DeleteAsync(Guid id);
    }
}
