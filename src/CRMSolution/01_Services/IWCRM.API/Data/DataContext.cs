using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;

namespace IWCRM.API.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder )
            => optionsBuilder.UseSqlite( connectionString: "DataSource=Data\\DataBase\\iwcrm.db;Cache=Shared" );

        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}
