using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Db
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Db.Order> Orders { get; set; }
        public DbSet<Db.OrderItem> OrderItems { get; set; }

        public OrdersDbContext(DbContextOptions options) : base(options) {}
    }
}
