using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.DataAccess.Entities;


namespace FoodDelivery.BusinessLogic.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<Dish>> GetAllAsync(string[] categories, bool? vegetarianOnly,
                                            int? sortBy, int page, int pageSize);
        Task<Dish> GetByIdAsync(Guid id);
        Task<Dish> CreateAsync(Dish dish);
        Task<Dish> UpdateAsync(Guid id, Dish dish);
        Task<bool> DeleteAsync(Guid id);
    }
}
