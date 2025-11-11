using FoodDelivery.BusinessLogic.DTOs;
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

        // ============================================================
        // GET ALL DISHES (with filters, sorting, pagination)
        // ============================================================
        public async Task<(IEnumerable<DishDto> Items, int Total)> GetAllAsync(
            string[]? categories,
            bool? vegetarianOnly,
            int? sortBy,
            int page,
            int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Dishes
                .AsNoTracking()
                .Include(d => d.DishCategory)
                .AsQueryable();

            // Filter by category names if provided
            if (categories is { Length: > 0 })
            {
                var norm = categories
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim().ToLower())
                    .ToArray();

                if (norm.Length > 0)
                    query = query.Where(d => norm.Contains(d.DishCategory.Name.ToLower()));
            }

            if (vegetarianOnly == true)
                query = query.Where(d => d.IsVegetarian);

            // sortBy: 0/NULL = by name asc, 1 = price asc, 2 = price desc
            query = sortBy switch
            {
                1 => query.OrderBy(d => d.Price).ThenBy(d => d.Name),
                2 => query.OrderByDescending(d => d.Price).ThenBy(d => d.Name),
                _ => query.OrderBy(d => d.Name)
            };

            var total = await query.CountAsync();

            // ✅ Fixed projection here
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
                    DishCategoryName = d.DishCategory.Name
                })
                .ToListAsync();

            return (items, total);
        }

        // ============================================================
        // GET DISH BY ID
        // ============================================================
        public async Task<DishDto?> GetByIdAsync(Guid id)
        {
            var dish = await _context.Dishes
                .AsNoTracking()
                .Include(d => d.DishCategory)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish is null) return null;

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Image = dish.Image,
                IsVegetarian = dish.IsVegetarian,
                DishCategoryId = dish.DishCategoryId,
                DishCategoryName = dish.DishCategory.Name
            };
        }

        // ============================================================
        // CREATE NEW DISH
        // ============================================================
        public async Task<DishDto> CreateAsync(CreateDishDto dto)
        {
            var category = await _context.DishCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.DishCategoryId);

            if (category == null)
                throw new ArgumentException("DishCategoryId does not exist.");

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

            return new DishDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Image = entity.Image,
                IsVegetarian = entity.IsVegetarian,
                DishCategoryId = entity.DishCategoryId,
                DishCategoryName = category.Name
            };
        }

        // ============================================================
        // UPDATE DISH
        // ============================================================
        public async Task<DishDto> UpdateAsync(Guid id, CreateDishDto dto)
        {
            var entity = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            if (entity is null)
                throw new KeyNotFoundException("Dish not found.");

            var category = await _context.DishCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.DishCategoryId);

            if (category == null)
                throw new ArgumentException("DishCategoryId does not exist.");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.Image = dto.Image;
            entity.IsVegetarian = dto.IsVegetarian;
            entity.DishCategoryId = dto.DishCategoryId;

            await _context.SaveChangesAsync();

            return new DishDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Image = entity.Image,
                IsVegetarian = entity.IsVegetarian,
                DishCategoryId = entity.DishCategoryId,
                DishCategoryName = category.Name
            };
        }

        // ============================================================
        // DELETE DISH
        // ============================================================
        public async Task DeleteAsync(Guid id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            if (dish is null)
                throw new KeyNotFoundException("Dish not found.");

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }
    }
}
