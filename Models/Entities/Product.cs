namespace SalesManagementSystem.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }  // Mã sản phẩm: PR0001, PR0002
        public string Name { get; set; }          // Tên sản phẩm
        public string Unit { get; set; }          // Đơn vị tính
        public decimal PurchasePrice { get; set; } // Đơn giá nhập
        public decimal SalePrice { get; set; }     // Đơn giá bán
        public bool IsActive { get; set; }         // Trạng thái kích hoạt (Yes/No)
        public DateTime CreatedAt { get; set; }    // Ngày tạo
        public DateTime UpdatedAt { get; set; }    // Ngày cập nhật
        public int TaxRate { get; set; }           // Mức thuế chịu (8% hoặc 10%)
    }

}
