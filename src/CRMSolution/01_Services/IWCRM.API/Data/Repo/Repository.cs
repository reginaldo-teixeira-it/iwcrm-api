using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace IWCRM.API.Data.Repo
{
    public class Repository
    {
 
        public static List<Person> GetAll()
        {
            var result = new List<Person>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite( "DataSource=iwcrm.db;Cache=Shared" ) // Certifique-se de ter a connectionString configurada corretamente
                .Options;

            using (var context = new DataContext( options ))
            {
                result = context.Person.ToList();
            }

            return result;
        }



    }
}
