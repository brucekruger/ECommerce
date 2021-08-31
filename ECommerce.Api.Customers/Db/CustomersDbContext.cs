using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Db
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Db.Customer> Customers { get; set; }

        public CustomersDbContext(DbContextOptions options) : base(options) {}
    }
}
