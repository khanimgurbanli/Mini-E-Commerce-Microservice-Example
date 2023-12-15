namespace Order.API.Dtos
{
    public class CreateOrderItemsDto
    {
        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
