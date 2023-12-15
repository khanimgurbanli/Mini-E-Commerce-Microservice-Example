using Order.API.Enums;

namespace Order.API.Entities
{
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
