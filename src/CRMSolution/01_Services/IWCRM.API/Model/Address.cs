using System.ComponentModel.DataAnnotations;

namespace IWCRM.API.Model
{
    public class Address
    {
        [Key]
        public long Id { get; set; }
        public long IdPerson { get; set; }
        public string? AddressDescription { get; set; }
        public string? Number { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public long? UFId { get; set; }
        public long? CountryID { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobileNumber { get; set; }
    }
}
