namespace SalesManagementSystem.Models
{
    
        public class AddProductDto
        {
            public string ProductCode { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public decimal PurchasePrice { get; set; }
            public decimal SalePrice { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }    // Ngày tạo
            public DateTime UpdatedAt { get; set; }
            public int TaxRate { get; set; }
        }
    

}
