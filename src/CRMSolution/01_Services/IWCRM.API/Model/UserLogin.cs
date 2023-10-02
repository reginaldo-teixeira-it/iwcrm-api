using System.ComponentModel.DataAnnotations;

namespace IWCRM.API.Model
{
    public class UserLogin
    {
        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 20, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string Username { get; set; }

        [Required( ErrorMessage = "Este campo é obrigatório" )]
        [MaxLength( 20, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        [MinLength( 3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres" )]
        public string Password { get; set; }
    }


}
