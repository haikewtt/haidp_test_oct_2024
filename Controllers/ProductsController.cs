using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesManagementSystem.Data;
using SalesManagementSystem.Models;
using SalesManagementSystem.Models.Entities;

namespace SalesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SalesDbContext dbContext;

        public ProductsController(SalesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var allProduct = dbContext.Products.ToList();

            return Ok(allProduct);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetProductById(int id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("paged")]
        public IActionResult GetPagedProducts(int pageNumber = 1, int pageSize = 10)
        {
            var product = dbContext.Products
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            return Ok(product);
        }

        [HttpGet("search")]
        public IActionResult SearchProducts(string searchTerm)
        {
            var product = dbContext.Products
                                .Where(p => p.Name.Contains(searchTerm) || p.ProductCode.Contains(searchTerm))
                                .ToList();
            if (product == null) { return NotFound(); }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductDto addProductDto)
        {
            var productEntity = new Product()
            {
                Name = addProductDto.Name,
                ProductCode = addProductDto.ProductCode,
                Unit = addProductDto.Unit,
                SalePrice = addProductDto.SalePrice,
                PurchasePrice = addProductDto.PurchasePrice,
                IsActive = addProductDto.IsActive,
                CreatedAt = addProductDto.CreatedAt,
                UpdatedAt = addProductDto.UpdatedAt,
                TaxRate = addProductDto.TaxRate
            };
            if (productEntity.PurchasePrice < 0 || productEntity.SalePrice < 0)
            {
                return BadRequest("Đơn giá nhập và đơn giá bán không được là số âm.");
            }

            productEntity.ProductCode = GenerateProductCode();
            productEntity.IsActive = true;
            productEntity.CreatedAt = DateTime.Now;

            dbContext.Products.Add(productEntity);
            dbContext.SaveChanges();

            return Ok(productEntity);
        }

        private string GenerateProductCode()
        {
            int nextId = dbContext.Products.Count() + 1;
            return $"PR{nextId:D4}";
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            var product = dbContext.Products.Find(id);

            if (product is null)
            {
                return NotFound();
            }

            if (updateProductDto.ProductCode != product.ProductCode)
            {
                return BadRequest("Can not change the product code");
            }

            if (updateProductDto.PurchasePrice < 0 || updateProductDto.SalePrice < 0)
            {
                return BadRequest("Sale price and purchase price can not be negative");
            }

            product.Name = updateProductDto.Name;
            product.Unit = updateProductDto.Unit;
            product.SalePrice = updateProductDto.SalePrice;
            product.PurchasePrice = updateProductDto.PurchasePrice;
            product.IsActive = updateProductDto.IsActive;
            product.UpdatedAt = DateTime.Now;
            product.TaxRate = updateProductDto.TaxRate;

            dbContext.SaveChanges();
            return Ok(product);
        }

        [HttpPut]
        [Route("toggle-active/{id:int}")]
        public IActionResult ToggleActiveStatus(int id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null) return NotFound();

            product.IsActive = !product.IsActive;
            product.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = dbContext.Products.Find(id);

            if (product is null) return NotFound();

            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("delete-multiple")]
        public IActionResult DeleteMultipleProducts([FromBody] List<string> productsName)
        {
            var products = dbContext.Products.Where(p => productsName.Contains(p.Name)).ToList();
            dbContext.Products.RemoveRange(products);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}
