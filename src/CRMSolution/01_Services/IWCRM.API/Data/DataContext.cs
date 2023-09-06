using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;

namespace IWCRM.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> options ) : base( options )
        {
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
