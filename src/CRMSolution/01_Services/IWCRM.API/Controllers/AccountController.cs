using IWCRM.API.Data;
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
		public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context,[FromBody] UserLogin model )
		{
			var user = await context.User
				.AsNoTracking()
				.Where(x => x.Username == model.Username && x.Password == model.Password)
				.FirstOrDefaultAsync();

			if (user == null)
				return NotFound(new { message = "Usuário ou senha inválidos" });

			var token = ServiceToken.GenerateToken(user);
			var refneshToken = ServiceToken.RefreshToken();
			ServiceToken.SaveRefreshToken(user.Username, refneshToken);

			// Esconde a senha
			user.Password = string.Empty;
			return new
			{
				user = user,
                accessToken = token,
                refreshToken = refneshToken
			};
		}

		[HttpPost]
		[Route("refresh-token")]
        public async Task<ActionResult<dynamic>> RefreshToken( [FromBody] RefreshTokenModel model )
        {
			var principal = ServiceToken.GetPrincipalFromExpiredToken( model.AccessToken );
			var username = principal.Identity.Name;
			var saveRefreshToken = ServiceToken.GetRefreshToken(username);
			if (saveRefreshToken != model.RefreshToken)
				throw new SecurityException("Inválid refresh token");

			var newJwtToken = ServiceToken.GenerateToken(principal.Claims);
			var newRefreshToken = ServiceToken.RefreshToken();
			ServiceToken.DeleteRefreshToken(username, newJwtToken);
			ServiceToken.SaveRefreshToken(username, newRefreshToken);

			return new ObjectResult(new
			{
				newtoken = newJwtToken,
				newrefreshToken = newRefreshToken
			});

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
			return users;
		}
	}
}
