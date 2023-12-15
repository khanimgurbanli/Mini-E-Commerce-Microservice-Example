
using Shared.Events.Common;
using Shared.Messages;

namespace Shared.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
