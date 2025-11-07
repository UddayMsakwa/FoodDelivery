using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;



namespace FoodDelivery.BusinessLogic.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync(string[] categories, bool? vegetarianOnly,
                                                         int? sortBy, int page, int pageSize)
        {
            var query = _context.Dishes
                .Include(d => d.Category)
                .AsQueryable();

            if (categories != null && categories.Length > 0)
                query = query.Where(d => categories.Contains(d.Category.Name));

            if (vegetarianOnly.HasValue && vegetarianOnly.Value)
                query = query.Where(d => d.IsVegetarian);

            // sortBy: 1 = name asc, 2 = price asc, 3 = price desc
            query = sortBy switch
            {
                1 => query.OrderBy(d => d.Name),
                2 => query.OrderBy(d => d.Price),
                3 => query.OrderByDescending(d => d.Price),
                _ => query
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Dish> GetByIdAsync(Guid id)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Dish> CreateAsync(Dish dish)
        {
            dish.Id = Guid.NewGuid();
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return dish;
        }

        public async Task<Dish> UpdateAsync(Guid id, Dish updated)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return null;

            dish.Name = updated.Name;
            dish.Description = updated.Description;
            dish.Price = updated.Price;
            dish.IsVegetarian = updated.IsVegetarian;
            dish.CategoryId = updated.CategoryId;

            await _context.SaveChangesAsync();
            return dish;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return false;

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
