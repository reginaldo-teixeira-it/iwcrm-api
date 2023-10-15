using IWCRM.API.Model;
using Microsoft.EntityFrameworkCore;

namespace IWCRM.API.Data.Repo
{
    public class AddressRepository
    {
        private static object databaseLock = new object();
        public static DbContextOptions<DataContext> GetConfiguration()
        {
            return new DbContextOptionsBuilder<DataContext>()
           .UseSqlite( "DataSource=iwcrm.db;Cache=Shared" )
           .Options;
        }

        #region Crud Person

        public static async Task<Address> Create( Address model )
        {
            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        dbContext.Address.Add( model );
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

        public static async Task<Address> Update( Address model )
        {

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        var address = dbContext.Address.Where( x => x.Id == model.Id ).FirstOrDefault();
                        if (address != null)
                        {
                            address.IdPerson = model.IdPerson;
                            address.AddressDescription = model.AddressDescription;
                            address.Number = model.Number;
                            address.PostalCode = model.PostalCode;
                            address.City = model.City;
                            address.UFId = model.UFId;
                            address.CountryID = model.CountryID;
                            address.PhoneNumber = model.PhoneNumber;
                            address.MobileNumber = model.MobileNumber;
                            address.Email = model.Email;
                            dbContext.Address.Update( address );
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

        public static async Task<Address> Delete( int id )
        {
            var addressRemove = new Address();

            try
            {
                lock (databaseLock)
                {
                    using (var dbContext = new DataContext( GetConfiguration() ))
                    {
                        addressRemove = dbContext.Address.Where( x => x.Id == id ).FirstOrDefault();
                        dbContext.Address.Remove( addressRemove );
                        dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return addressRemove;

        }

        public static async Task<Address> GetById( int Id )
        {
            var address = new Address();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                address = await dbContext.Address
                    .AsNoTracking()
                    .Where( x => x.Id == Id )
                    .FirstOrDefaultAsync();
                dbContext.Dispose();
            }

            return address;
        }

        public static async Task<List<Address>> GetAll()
        {
            var address = new List<Address>();

            using (var dbContext = new DataContext( GetConfiguration() ))
            {
                address = await dbContext
                .Address
                .AsNoTracking()
                .ToListAsync();

                dbContext.Dispose();
            }

            return address;
        }

        #endregion
    }
}
