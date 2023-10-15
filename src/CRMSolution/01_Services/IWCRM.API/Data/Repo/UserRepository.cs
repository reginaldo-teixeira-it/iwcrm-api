using IWCRM.API.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;

namespace IWCRM.API.Data.Repo
{
    public class UserRepository
    {
        private static object databaseLock = new object();
        public static DbContextOptions<DataContext> GetConfiguration()
        {   return new DbContextOptionsBuilder<DataContext>()
           .UseSqlite( "DataSource=iwcrm.db;Cache=Shared" )  
           .Options;
        }
  
        public static async Task<User> GetUserAsync( UserLogin model )
        {
            var user = new User();
 
                using (var dbContext = new DataContext( GetConfiguration() ))
                {
                    user = await dbContext.User
                        .AsNoTracking()
                        .Where( x => x.Username == model.Username && x.Password == model.Password )
                        .FirstOrDefaultAsync();
                    dbContext.Dispose();
                }           

            return user;
        }

        public static async Task<User> GetByIdAsync( int Id )
        {
            var user = new User();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                user = await dbContext.User
                    .AsNoTracking()
                    .Where( x => x.Id == Id )
                    .FirstOrDefaultAsync();
                dbContext.Dispose();
            }

            return user;
        }

        public static async Task<List<User>> GetUsersAsync()
        {
            var users = new List<User>();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                users = await dbContext
                .User
                .AsNoTracking()
                .ToListAsync();

                dbContext.Dispose();
            }

            return users;
        }

        public static async Task<User> Create( User model )
        {
 
            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        dbContext.User.Add( model );
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return model;

        }

        public static async Task<User> Update( User model )
        {

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        var user = dbContext.User.Where( x => x.Id == model.Id ).FirstOrDefault();
                        if (user != null)
                        {
                            user.Username = model.Username;
                            user.Password = model.Password;
                            user.Email = model.Email; 
                            user.Role = model.Role;
                            user.AccessToken = model.AccessToken;
                            user.RefreshToken = model.RefreshToken;
                            dbContext.User.Update( user );
                            dbContext.SaveChangesAsync();
                            dbContext.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return model;

        }

        public static async Task<User> Delete( int id )
        {
            var userRemove = new User();

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        userRemove = dbContext.User.Where( x => x.Id == id ).FirstOrDefault();
                        dbContext.User.Remove( userRemove );
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return userRemove;

        }

        public static List<Person> GetAll()
        {
            var result = new List<Person>();
 
            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                result = dbContext.Person.ToList();
                dbContext.Dispose();
            }
            return result;
        }

        public static void SaveRefreshToken( string username, string accessToken, string refreshToken )
        {
            lock (databaseLock)
            {
                using (var dbContext = new DataContext( GetConfiguration() ))
                {
                    var user = dbContext.User.Where( x => x.Username == username ).FirstOrDefault();
                    if (user != null)
                    {
                        user.RefreshToken = refreshToken;
                        user.AccessToken = accessToken;
                        dbContext.User.Update( user );
                        dbContext.SaveChangesAsync();
                        dbContext.Dispose();
                    }
                }
            }
        }

        public static void DeleteRefreshToken( string username )
        {
            lock (databaseLock)
            {
                using (var dbContext = new DataContext( GetConfiguration() ))
                {
                    var user = dbContext.User.Where( x => x.Username == username ).FirstOrDefault();
                    if (user != null)
                    {
                        user.RefreshToken = string.Empty;
                        user.AccessToken = string.Empty;
                        dbContext.User.Update( user );
                        dbContext.SaveChangesAsync();
                        dbContext.Dispose();
                    }
                }
            }
        }

        public static async Task<string> GetRefreshToken( string username )
        {
            string refreshToken = string.Empty;

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                var result = await dbContext.User.Where( x => x.Username == username ).FirstOrDefaultAsync();
                dbContext.Dispose();
                refreshToken = result?.RefreshToken;
            }

            return refreshToken;
        }
    }
}
