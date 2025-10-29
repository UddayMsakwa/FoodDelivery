using Microsoft.EntityFrameworkCore;
using FoodDelivery.DataAccess.Entities;

namespace FoodDelivery.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<DishCategory> DishCategories => Set<DishCategory>();
    }
}
