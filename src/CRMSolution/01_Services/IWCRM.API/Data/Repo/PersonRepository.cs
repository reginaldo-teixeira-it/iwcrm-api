using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace IWCRM.API.Data.Repo
{
    public class PersonRepository
    {
        private static object databaseLock = new object();
        public static DbContextOptions<DataContext> GetConfiguration()
        {
            return new DbContextOptionsBuilder<DataContext>()
           .UseSqlite( "DataSource=iwcrm.db;Cache=Shared" )
           .Options;
        }


        #region Crud Person

        public static async Task<Person> Create( Person model )
        {

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        model.CompleteName = string.Concat( model.FirstName, " ", model.MiddleName, " ", model.LastName ).Trim();
                        dbContext.Person.Add( model );
                        dbContext.SaveChangesAsync();
                        dbContext.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return model;

        }

        public static async Task<Person> Update( Person model )
        {

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        var person = dbContext.Person.Where( x => x.Id == model.Id ).FirstOrDefault();
                        if (person != null)
                        {
                            person.FirstName = model.FirstName;
                            person.MiddleName = model.MiddleName;
                            person.LastName = model.LastName;
                            person.CPFCNPJ = model.CPFCNPJ;
                            person.RGIE = model.RGIE;
                            dbContext.Person.Update( person );
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

        public static async Task<Person> Delete( int id )
        {
            var personRemove = new Person();

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        personRemove = dbContext.Person.Where( x => x.Id == id ).FirstOrDefault();
                        dbContext.Person.Remove( personRemove );
                        dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return personRemove;

        }

        public static async Task<Person> GetById( int Id )
        {
            var user = new Person();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                user = await dbContext.Person
                    .AsNoTracking()
                    .Where( x => x.Id == Id )
                    .FirstOrDefaultAsync();
                dbContext.Dispose();
            }

            return user;
        }

        public static async Task<List<Person>> GetAll()
        {
            var users = new List<Person>();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                users = await dbContext
                .Person
                .AsNoTracking()
                .ToListAsync();

                dbContext.Dispose();
            }

            return users;
        }

        #endregion

    }
}
