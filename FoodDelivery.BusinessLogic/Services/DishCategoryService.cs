using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.BusinessLogic.Services
{
    public class DishCategoryService : IDishCategoryService
    {
        private readonly ApplicationDbContext _context;

        public DishCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DishCategoryDto>> GetAllAsync()
        {
            return await _context.DishCategories
                .Select(c => new DishCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<DishCategoryDto> GetByIdAsync(Guid id)
        {
            var category = await _context.DishCategories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            return new DishCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<DishCategoryDto> CreateAsync(CreateDishCategoryDto dto)
        {
            var entity = new DishCategory
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            _context.DishCategories.Add(entity);
            await _context.SaveChangesAsync();

            return new DishCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public async Task<DishCategoryDto> UpdateAsync(Guid id, CreateDishCategoryDto dto)
        {
            var category = await _context.DishCategories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            category.Name = dto.Name;
            await _context.SaveChangesAsync();

            return new DishCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.DishCategories.FindAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            _context.DishCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
