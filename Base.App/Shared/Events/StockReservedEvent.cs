using Shared.Events.Common;

namespace Shared.Events
{
    public class StockReservedEvent : IEvent
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
