using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesManagementSystem.Data;
using SalesManagementSystem.Models;
using SalesManagementSystem.Models.Entities;
using System;

namespace SalesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SalesDbContext dbContext;

        public OrdersController(SalesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var allOrder = dbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToList();

            return Ok(allOrder);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetOrder (int id)
        {
            var order = dbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public IActionResult AddOrder(CreateOrderDto addOrderDto)
        {
            var today = DateTime.Now;
            var dateCode = today.ToString("ddMMyy");
            var orderCountToday = dbContext.Orders.Count(o => o.CreatedAt.Date == today.Date) + 1;
            var orderCode = $"OR{dateCode}{orderCountToday:D3}";

            var orderEntity = new Order()
            {
                OrderCode = orderCode,
                CustomerName = addOrderDto.CustomerName,
                CustomerPhone = addOrderDto.CustomerPhone,
                CreatedAt = DateTime.Now,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            foreach (var itemDto in addOrderDto.Items)
            {
                var product = dbContext.Products.Find(itemDto.ProductId);
                if (product == null)
                {
                    return StatusCode(403, $"Product with ID {itemDto.ProductId} does not exist.");
                }
                var salePrice = itemDto.SalePrice > 0 ? itemDto.SalePrice : product.SalePrice;

                var linePrice = salePrice * itemDto.Quantity;
                var lineTax = linePrice * product.TaxRate / 100;
                var lineTotal = linePrice - lineTax;

                orderEntity.OrderItems.Add(new OrderItem()
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        SalePrice = salePrice,
                        TaxRate = product.TaxRate,
                        TotalLinePrice = linePrice,
                        TotalLineTax = lineTax

                    }
                );

                orderEntity.TotalAmount += lineTotal;
                orderEntity.TotalTax += lineTax;

            }

            dbContext.Orders.Add(orderEntity);
            dbContext.SaveChanges();

            return Ok(orderEntity);
        }
        

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] CreateOrderDto orderDto)
        {
            var order = await dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();

            
            order.CustomerName = orderDto.CustomerName;
            order.CustomerPhone = orderDto.CustomerPhone;

            
            dbContext.OrderItems.RemoveRange(order.OrderItems);
            order.OrderItems.Clear();
            order.TotalAmount = 0;
            order.TotalTax = 0;

            foreach (var itemDto in orderDto.Items)
            {
                var product = await dbContext.Products.FindAsync(itemDto.ProductId);
                if (product == null) return StatusCode(403, $"Product with ID {itemDto.ProductId} does not exist.");

                var salePrice = itemDto.SalePrice > 0 ? itemDto.SalePrice : product.SalePrice;

                var linePrice = salePrice * itemDto.Quantity;
                var lineTax = linePrice * product.TaxRate / 100;
                var lineTotal = linePrice - lineTax;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    SalePrice = salePrice,
                    TaxRate = product.TaxRate,
                    TotalLinePrice = linePrice,
                    TotalLineTax = lineTax
                });

                order.TotalAmount += lineTotal;
                order.TotalTax += lineTax;
            }

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }

}
