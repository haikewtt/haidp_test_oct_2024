namespace SalesManagementSystem.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }       // Mã đơn hàng: OD251024001, OD251024002
        public string CustomerName { get; set; }    // Tên khách hàng
        public string CustomerPhone { get; set; }   // Số điện thoại khách hàng
        public decimal TotalAmount { get; set; }    // Thành tiền đơn hàng
        public decimal TotalTax { get; set; }       // Tổng số thuế của đơn hàng
        public DateTime CreatedAt { get; set; }     // Ngày tạo đơn
        public ICollection<OrderItem> OrderItems { get; set; } // Danh sách sản phẩm
    }
}
