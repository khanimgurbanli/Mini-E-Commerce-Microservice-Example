

using Shared.Events.Common;

namespace Shared.Events
{
    public class PaymentComplatedEvent : IEvent
    {
        public string ConsumerId { get; set; }
        public string OrderId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
