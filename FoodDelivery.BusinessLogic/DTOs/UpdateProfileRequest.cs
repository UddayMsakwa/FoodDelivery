using System;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class UpdateProfileRequest
    {
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
