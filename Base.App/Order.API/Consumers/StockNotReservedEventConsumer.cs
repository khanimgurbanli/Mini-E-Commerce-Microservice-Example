using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly OrderDbContext _context;
        public StockNotReservedEventConsumer(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Order.API.Entities.Order order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus = Enums.OrderStatus.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
