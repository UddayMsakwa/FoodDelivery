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
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<DishDto> Items, int Total)> GetAllAsync(string[]? categories, bool? vegetarianOnly, int? sortBy, int page, int pageSize)
        {
            var query = _context.Dishes
                .Include(d => d.DishCategory)
                .AsQueryable();

            if (categories != null && categories.Length > 0)
                query = query.Where(d => categories.Contains(d.DishCategory.Name));

            if (vegetarianOnly.HasValue && vegetarianOnly.Value)
                query = query.Where(d => d.IsVegetarian);

            // Sorting: 1 = Name ASC, 2 = Price ASC, 3 = Price DESC
            query = sortBy switch
            {
                1 => query.OrderBy(d => d.Name),
                2 => query.OrderBy(d => d.Price),
                3 => query.OrderByDescending(d => d.Price),
                _ => query.OrderBy(d => d.Name)
            };

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DishDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Image = d.Image,
                    IsVegetarian = d.IsVegetarian,
                    DishCategoryId = d.DishCategoryId,
                    CategoryName = d.DishCategory.Name
                })
                .ToListAsync();

            return (items, total);
        }

        public async Task<DishDto?> GetByIdAsync(Guid id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishCategory)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
                return null;

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                IsVegetarian = dish.IsVegetarian,
                DishCategoryId = dish.DishCategoryId,
                CategoryName = dish.DishCategory.Name
            };
        }

        public async Task<DishDto> CreateAsync(CreateDishDto dto)
        {
            var entity = new Dish
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                IsVegetarian = dto.IsVegetarian,
                DishCategoryId = dto.DishCategoryId
            };

            _context.Dishes.Add(entity);
            await _context.SaveChangesAsync();

            var category = await _context.DishCategories.FindAsync(dto.DishCategoryId);

            return new DishDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Image = entity.Image,
                IsVegetarian = entity.IsVegetarian,
                DishCategoryId = entity.DishCategoryId,
                CategoryName = category?.Name ?? string.Empty
            };
        }

        public async Task<DishDto> UpdateAsync(Guid id, CreateDishDto dto)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");

            dish.Name = dto.Name;
            dish.Description = dto.Description;
            dish.Price = dto.Price;
            dish.Image = dto.Image;
            dish.IsVegetarian = dto.IsVegetarian;
            dish.DishCategoryId = dto.DishCategoryId;

            await _context.SaveChangesAsync();

            var category = await _context.DishCategories.FindAsync(dto.DishCategoryId);

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                IsVegetarian = dish.IsVegetarian,
                DishCategoryId = dish.DishCategoryId,
                CategoryName = category?.Name ?? string.Empty
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
                throw new KeyNotFoundException("Dish not found");

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }
    }
}
