using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Contexts;
using Order.API.Dtos;
using Order.API.Enums;
using Shared.Events;
using Shared.Messages;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto request)
        {
            Order.API.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerId = request.CustomerId,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatus.Suspend
            };

            order.OrderItems = request.CreateOrderItems.Select(o => new Entities.OrderItem
            {
                Count = o.Count,
                Price = o.Price,
                ProductId = o.ProductId,
            }).ToList();

            order.TotalPrice = request.CreateOrderItems.Sum(o => (o.Price * o.Count));

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                CustomerId = order.CustomerId,
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemMessage
                {
                    ProductId = oi.ProductId,
                    Count = oi.Count,
                }).ToList()
            };

            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok();
        }
    }
}
