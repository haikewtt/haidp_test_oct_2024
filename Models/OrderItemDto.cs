namespace SalesManagementSystem.Models
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
    }
}
