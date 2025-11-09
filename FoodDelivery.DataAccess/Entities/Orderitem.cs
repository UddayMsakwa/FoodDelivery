using System;

namespace FoodDelivery.DataAccess.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }            
        public Guid OrderId { get; set; }       
        public Guid DishId { get; set; }        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order Order { get; set; } = null!;
        public Dish Dish { get; set; } = null!;
    }
}
