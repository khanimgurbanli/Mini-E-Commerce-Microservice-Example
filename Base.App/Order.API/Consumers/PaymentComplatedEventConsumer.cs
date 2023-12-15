using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentComplatedEventConsumer : IConsumer<PaymentComplatedEvent>
    {
        private readonly OrderDbContext _context;
        public PaymentComplatedEventConsumer(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentComplatedEvent> context)
        {
            Order.API.Entities.Order order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus = Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}
