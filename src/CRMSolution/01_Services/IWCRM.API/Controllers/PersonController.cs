using IWCRM.API.Data;
using IWCRM.API.Data.Repo;
using IWCRM.API.Model;
using IWCRM.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IWCRM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route( "v1/person" )]
    public class PersonController : Controller
    {
        [HttpGet]
        [Route( "get-all" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<List<Person>>> GetAll( [FromServices] DataContext context )
        {
            var result = Repository.GetAll();
            // return await context.Person.AsNoTracking().ToListAsync();
            return result;
        }

        [HttpGet]
        [Route( "get-byid/{id:int}" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Person>> GetById( [FromServices] DataContext context, int id )
        {
            return await context.Person.AsNoTracking().FirstOrDefaultAsync( x => x.Id == id );
        }

        [HttpPost]
        [Route( "create" )]
        [Authorize( Roles = "Administrator" )]

        public async Task<ActionResult<Person>> Create( [FromServices] DataContext context,[FromBody] Person model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {  
                context.Person.Add( model );
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

        public async Task<ActionResult<Person>> Update( [FromServices] DataContext context, [FromBody] Person model, int id)
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

        public async Task<ActionResult<Person>> Delete( [FromServices] DataContext context, [FromBody] Person model, int id )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound( new { message = "Pessoa não encontrada" } );

            try
            {
                model.IsActive = false;
                context.Entry( model ).State = EntityState.Modified;
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
