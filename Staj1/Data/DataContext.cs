using Microsoft.EntityFrameworkCore;
namespace Staj1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
    }
}
