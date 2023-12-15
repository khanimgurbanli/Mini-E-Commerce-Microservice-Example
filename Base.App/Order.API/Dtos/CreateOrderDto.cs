namespace Order.API.Dtos
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; }
        public ICollection<CreateOrderItemsDto> CreateOrderItems { get; set; }
    }
}
