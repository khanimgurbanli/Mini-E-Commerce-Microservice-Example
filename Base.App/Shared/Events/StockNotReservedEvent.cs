using Shared.Events.Common;

namespace Shared.Events
{
    public class StockNotReservedEvent : IEvent
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string Message { get; set; }
    }
}
