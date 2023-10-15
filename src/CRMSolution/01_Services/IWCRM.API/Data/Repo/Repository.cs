﻿using IWCRM.API.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

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
  
        public static User GetUser( UserLogin model )
        {
            var user = new User();

            using (var context = new DataContext( GetConfiguration() ))
            {
                user = context.User
                    .AsNoTracking()
                    .Where( x => x.Username == model.Username && x.Password == model.Password )
                    .FirstOrDefault();
                    context.Dispose();
            }

            return user;
        }


        public static List<Person> GetAll()
        {
            var result = new List<Person>();
 
            using (var context = new DataContext( GetConfiguration() ))
            {
                result = context.Person.ToList();
                context.Dispose();
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
                    context.Dispose();
                }
            }
        }
 
    }
}
