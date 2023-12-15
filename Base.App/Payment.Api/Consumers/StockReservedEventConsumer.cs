using MassTransit;
using Shared.Events;

namespace Payment.Api.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publish;

        public StockReservedEventConsumer(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)//declare to situation
            {
                PaymentComplatedEvent paymentComplatedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    ConsumerId = context.Message.CustomerId,
                    TotalPrice = context.Message.TotalPrice,
                };
                _publish.Publish(paymentComplatedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Error message "
                };
                _publish.Publish(paymentFailedEvent);   
            }

            return Task.CompletedTask;
        }
    }
}
