using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IWCRM.API.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 20, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string Username { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 20, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string Password { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 100, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string Email { get; set; }

        public string Role { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 300, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 10, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string AccessToken { get; set; }
        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 300, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 10, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string RefreshToken { get; set; }
    }
}
