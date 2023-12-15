using Shared.Events.Common;

namespace Shared.Events
{
    public class PaymentFailedEvent : IEvent
    {
        public string OrderId { get; set; }
        public string Message { get; set; }
    }
}
