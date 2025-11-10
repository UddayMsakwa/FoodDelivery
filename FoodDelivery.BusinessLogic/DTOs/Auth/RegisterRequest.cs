namespace FoodDelivery.BusinessLogic.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
