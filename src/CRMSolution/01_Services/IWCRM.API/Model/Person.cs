using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace IWCRM.API.Model
{
    public class Person
    {
        [Key]
        public long  Id { get; set; }
        public long IdPersonType { get; set; }
        public long IdAddress { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 14, ErrorMessage = "Este campo deve conter 14 caracteres" )]
        [MinLength( 11, ErrorMessage = "Este campo deve conter 14 caracteres" )]
        public string CPFCNPJ { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 11, ErrorMessage = "Este campo deve conter 11 caracteres" )]
        [MinLength( 8, ErrorMessage = "Este campo deve conter 11 caracteres" )]
        public string RGIE { get; set; }
    }
}
