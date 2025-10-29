using System;

namespace FoodDelivery.BusinessLogic.DTOs
{
    public class CheckoutRequest
    {
        public string DeliveryAddress { get; set; } = null!;
        public DateTime DeliveryTime { get; set; }
    }
}
