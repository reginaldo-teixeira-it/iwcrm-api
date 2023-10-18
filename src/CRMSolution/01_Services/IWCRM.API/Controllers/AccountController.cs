using IWCRM.API.Data;
using IWCRM.API.Data.Repo;
using IWCRM.API.Model;
using IWCRM.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace IWCRM.API.Controllers
{
	[Route("v1/account")]
	public class AccountController : Controller
	{
		[HttpPost]
		[Route( "signin" )]
		[AllowAnonymous]
		public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserLogin model )
		{
            var user = UserRepository.GetUserAsync( model ).Result;
            //var user = await context.User
            //    .AsNoTracking()
            //    .Where( x => x.Username == model.Username && x.Password == model.Password )
            //    .FirstOrDefaultAsync();

            if (user == null)
                return NotFound( new { errorMessage = "Usuário ou senha inválidos" } );

            var accessToken = ServiceToken.GenerateToken( user );
            var refneshToken = ServiceToken.RefreshToken();
            try
            {
                UserRepository.SaveRefreshToken( user.Username, accessToken, refneshToken );
            }
            catch (Exception ex)
            {

                return BadRequest( ex.Message );
            }

            // Esconde a senha
            user.Password = string.Empty;
            return new
            {
                //user = user,
                accessToken = accessToken,
                refreshToken = refneshToken
            };
        }

		[HttpPut]
		[Route("refresh-token")]
        public async Task<ActionResult<dynamic>> RefreshToken( [FromBody] RefreshTokenModel model )
        {
            var principal = ServiceToken.GetPrincipalFromExpiredToken( model.AccessToken );
            var username = principal.Identity.Name;
            var saveRefreshToken = UserRepository.GetRefreshToken( username );
            if (saveRefreshToken.Result != model.RefreshToken)
                throw new SecurityException( "Inválid refresh token" );

            var newJwtToken = ServiceToken.GenerateToken( principal.Claims );
            var newRefreshToken = ServiceToken.RefreshToken();
            UserRepository.DeleteRefreshToken( username );
            UserRepository.SaveRefreshToken( username, newJwtToken, newRefreshToken );

            return new ObjectResult( new
            {
                newtoken = newJwtToken,
                newrefreshToken = newRefreshToken
            } );
        }

		[HttpGet]
		[Route("get-all")]
        [Authorize( Roles = "Administrator" )]
        [AllowAnonymous]
		public async Task<ActionResult<List<User>>> GetAll()
		{
            var users = new List<User>();
            try
            {
                users = await UserRepository.GetUsersAsync();
            }
            catch (Exception ex)
            {
                return BadRequest( new { message = "Não foi possível listar usuários "+ex.Message } );
            }
 
            return users;
		}

        [HttpGet]
        [Route( "get-byid" )]
        [Authorize( Roles = "Administrator" )]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetByID( int id )
        {
            var users = new User();
            try
            {
                users = await UserRepository.GetByIdAsync( id );
                if (users == null)
                {
                    return new ObjectResult( new { message = "Não foi possível encontrar o usuário" } )
                    {
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }
            }
            catch (Exception ex)
            {
                return BadRequest( new { message = "Não foi possível encontrar o usuário " + ex.Message } );
            }

            return users;
        }

        [HttpPost]
        [Route( "create" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<User>> CreateUsr([FromBody] User model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                UserRepository.Create( model );

                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o usuário" } );

            }
        }

        [HttpPut]
        [Route( "update" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<User>> UpdateUsr( [FromBody] User model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                UserRepository.Update( model );

                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível atualizar o usuário" } );

            }
        }

        [HttpDelete]
        [Route( "delete" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<User>> DeleteUsr( int id )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
               var result =  UserRepository.Delete( id ).Result;

                return result;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível remover o usuário" } );

            }
        }

    }
}
