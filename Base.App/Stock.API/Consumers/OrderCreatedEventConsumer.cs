using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Stock.API.Entities.Stock> _stockCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(MongoDbService mongoDbService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _stockCollection = mongoDbService.GetCollection<Stock.API.Entities.Stock>();
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> result = new();

            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                result.Add((await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Couunt >= orderItem.Count)).Any());
            }

            if (result.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage item in context.Message.OrderItems)
                {
                    Stock.API.Entities.Stock stock = await (await _stockCollection.FindAsync(s => s.ProductId == item.ProductId)).FirstOrDefaultAsync();

                    stock.Couunt -= item.Count;
                    await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);//update
                }

                StockReservedEvent stockReservedEvent = new()
                {
                    CustomerId = context.Message.CustomerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                };

                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue: {RabbitMQSettings.StockReservedEventQueue_Payment}"));
                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                StockReservedEvent stockReservedEvent = new()
                {
                    CustomerId = context.Message.CustomerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice,
                };
                await _publishEndpoint.Publish(stockReservedEvent);
            }
        }
    }
}
