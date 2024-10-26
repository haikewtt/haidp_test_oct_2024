namespace SalesManagementSystem.Models
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

}
