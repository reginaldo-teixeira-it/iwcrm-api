using IWCRM.API.Data;
using IWCRM.API.Model;
using IWCRM.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWCRM.API.Controllers
{
    [Route( "v1/account" )]
    public class AccountController : Controller
    {

        [HttpPost]
        [Route( "login" )]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext context,
            [FromBody] User model )
        {
            var user = await context.User
                .AsNoTracking()
                .Where( x => x.Username == model.Username && x.Password == model.Password )
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound( new { message = "Usuário ou senha inválidos" } );

            var token = ServiceToken.GenerateToken( user );
            // Esconde a senha
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route( "get-users" )]
        //[Authorize(Roles = "manager")]
        [AllowAnonymous]
        public async Task<ActionResult<List<User>>> Get( [FromServices] DataContext context )
        {
            var users = await context
                .User
                .AsNoTracking()
                .ToListAsync();
            return users;
        }
    }
}
