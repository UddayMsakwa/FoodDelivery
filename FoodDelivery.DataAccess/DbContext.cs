using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dish>().Property(d => d.Id).ValueGeneratedNever();
            modelBuilder.Entity<DishCategory>().Property(c => c.Id).ValueGeneratedNever();
            modelBuilder.Entity<Order>().Property(o => o.Id).ValueGeneratedNever();
            modelBuilder.Entity<Rating>().Property(r => r.Id).ValueGeneratedNever();

            // Dish -> Category
            modelBuilder.Entity<Dish>()
                .HasOne(d => d.DishCategory)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.DishCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Dish)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(oi => oi.DishId);

            // Rating relationships
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Dish)
                .WithMany(d => d.Ratings)
                .HasForeignKey(r => r.DishId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);
        }
    }
}
