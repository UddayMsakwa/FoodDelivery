using System;
using System.Collections.Generic;

namespace FoodDelivery.DataAccess.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        // ===== Credentials =====
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // ===== Personal Info =====
        public string Name { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        // ===== Relationships =====
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
