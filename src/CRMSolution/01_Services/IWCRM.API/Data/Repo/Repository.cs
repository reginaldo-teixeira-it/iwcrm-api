using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;

namespace IWCRM.API.Data.Repo
{
    public class Repository
    {
 
        public Repository(){}


        public static DbContextOptions<DataContext> GetConfiguration()
        {   return new DbContextOptionsBuilder<DataContext>()
           .UseSqlite( "DataSource=iwcrm.db;Cache=Shared" )  
           .Options;
        }
  
        public static List<Person> GetAll()
        {
            var result = new List<Person>();
 
            using (var context = new DataContext( GetConfiguration() ))
            {
                result = context.Person.ToList();
            }

            return result;
        }

        public static void SaveRefreshToken( string username, string accessToken, string refreshToken )
        {
            using (var context = new DataContext( GetConfiguration() ))
            {
                var user = context.User.Where( x => x.Username == username ).FirstOrDefault();
                if (user != null)
                {
                    user.RefreshToken = refreshToken;
                    user.AccessToken = accessToken;
                    context.User.Update( user );
                    context.SaveChanges();
                }
            } 
        }



    }
}
