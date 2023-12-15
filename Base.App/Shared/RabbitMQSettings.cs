
namespace Shared
{
    static public class RabbitMQSettings
    {
        public const string OrderCreatedEventQueue_Stock = "order-created-event-stock-queue";
        public const string StockReservedEventQueue_Payment = "stock-reserved-event-payment-queue";
        public const string PaymentReceivedEventQueue_Order = "payment-received-event-order-queue";
        public const string StockNotReceivedEventQueue_Order = "stock-notreceived-event-order-queue";
        public const string PaymentFailedEventQueue_Order = "order-payment-failed-event-queue";
    }
}
