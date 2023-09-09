using IWCRM.API.Data;
using IWCRM.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWCRM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route( "v1/address" )]

    public class AddresController : Controller
    {
        [HttpGet]
        [Route( "getByid/{id:int}" )]
        [Authorize( Roles = "Administrator" )]

        public async Task<ActionResult<Address>> GetById( [FromServices] DataContext context, int id )
        {
            return await context.Address.AsNoTracking().FirstOrDefaultAsync( x => x.Id == id );
        }

        [HttpPost]
        [Route( "create" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Address>> Create( [FromServices] DataContext context, [FromBody] Address model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                context.Address.Add( model );
                await context.SaveChangesAsync();

                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o usuário" } );

            }
        }

        [HttpPut]
        [Route( "update/{id:int}" )]
        [Authorize( Roles = "Administrator" )]

        public async Task<ActionResult<Address>> Update( [FromServices] DataContext context, [FromBody] Address model, int id )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound( new { message = "Pessoa não encontrada" } );

            try
            {
                context.Entry( model ).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o cadastro" } );

            }
        }

        [HttpPut]
        [Route( "delete/{id:int}" )]
        [Authorize( Roles = "Administrator" )]

        public async Task<ActionResult<Address>> Delete( [FromServices] DataContext context, [FromBody] Address model, int id )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            var category = await context.Address.FirstOrDefaultAsync( x => x.Id == id );
            if (category == null)
                return NotFound( new { message = "Endereço não encontrado" } );

            try
            {
                context.Address.Remove( category );
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o cadastro" } );

            }
        }

    }
}
