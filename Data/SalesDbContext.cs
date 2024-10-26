using Microsoft.EntityFrameworkCore;
using SalesManagementSystem.Models.Entities;

namespace SalesManagementSystem.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
