using FoodDelivery.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IDishCategoryService
    {
        Task<IEnumerable<DishCategoryDto>> GetAllAsync();
        Task<DishCategoryDto> GetByIdAsync(Guid id);
        Task<DishCategoryDto> CreateAsync(CreateDishCategoryDto dto);
        Task<DishCategoryDto> UpdateAsync(Guid id, CreateDishCategoryDto dto);
        Task DeleteAsync(Guid id);
    }
}
