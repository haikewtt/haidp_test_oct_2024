namespace SalesManagementSystem.Models.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }       // Đơn giá bán
        public int TaxRate { get; set; }             // Tỉ lệ thuế
        public decimal TotalLinePrice { get; set; }  // Thành tiền từng dòng
        public decimal TotalLineTax { get; set; }    // Thuế từng dòng
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
