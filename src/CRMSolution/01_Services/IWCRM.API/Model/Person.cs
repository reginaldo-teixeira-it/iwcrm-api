using System.Numerics;

namespace IWCRM.API.Model
{
    public class Person
    {
        public long  Id { get; set; }
        public long IdPersonType { get; set; }
        public long IdAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public string CPFCNPJ { get; set; }
        public string RGIE { get; set; }
    }
}
