using IWCRM.API.Data;
using IWCRM.API.Data.Repo;
using IWCRM.API.Model;
using IWCRM.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace IWCRM.API.Controllers
{
	[Route("v1/account")]
	public class AccountController : Controller
	{
		[HttpPost]
		[Route("login")]
		[AllowAnonymous]
		public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserLogin model )
		{
            var user = Repository.GetUserAsync( model ).Result;
            //var user = await context.User
            //    .AsNoTracking()
            //    .Where( x => x.Username == model.Username && x.Password == model.Password )
            //    .FirstOrDefaultAsync();

            if (user == null)
                return NotFound( new { message = "Usuário ou senha inválidos" } );

            var accessToken = ServiceToken.GenerateToken( user );
            var refneshToken = ServiceToken.RefreshToken();
            try
            {
                Repository.SaveRefreshToken( user.Username, accessToken, refneshToken );
            }
            catch (Exception ex)
            {

                return BadRequest( ex.Message );
            }

            // Esconde a senha
            user.Password = string.Empty;
            return new
            {
                user = user,
                accessToken = accessToken,
                refreshToken = refneshToken
            };
        }

		[HttpPost]
		[Route("refresh-token")]
        public async Task<ActionResult<dynamic>> RefreshToken( [FromServices] DataContext context, [FromBody] RefreshTokenModel model )
        {
			try
			{
                var principal = ServiceToken.GetPrincipalFromExpiredToken( model.AccessToken );
                var username = principal.Identity.Name;
                var saveRefreshToken = ServiceToken.GetRefreshToken( context, username );
                if (saveRefreshToken != model.RefreshToken)
                    throw new SecurityException( "Inválid refresh token" );

                var newJwtToken = ServiceToken.GenerateToken( principal.Claims );
                var newRefreshToken = ServiceToken.RefreshToken();
                ServiceToken.DeleteRefreshToken( context, username, newJwtToken );
                ServiceToken.SaveRefreshToken( context, username, newJwtToken, newRefreshToken );

                return new ObjectResult( new
                {
                    newtoken = newJwtToken,
                    newrefreshToken = newRefreshToken
                } );
            }
			catch (Exception)
			{
				throw;
			}
			finally
			{
                context.Dispose();
                context = null;
            }
		}

		[HttpGet]
		[Route("get-users")]
		//[Authorize(Roles = "manager")]
		[AllowAnonymous]
		public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
		{
			var users = await context
				.User
				.AsNoTracking()
				.ToListAsync();

            context.Dispose();
            context = null;

            return users;
		}
	}
}
