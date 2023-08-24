using Microsoft.EntityFrameworkCore;
using UserAuthApi.Entities;

namespace UserAuthApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
