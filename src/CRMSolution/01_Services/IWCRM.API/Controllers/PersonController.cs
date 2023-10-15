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
        public async Task<ActionResult<List<Person>>> GetAll()
        {
            var result = PersonRepository.GetAll().Result;
            return result;
        }

        [HttpGet]
        [Route( "get-byid/{id:int}" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Person>> GetById( int id )
        {
            return await PersonRepository.GetById( id );
        }

        [HttpPost]
        [Route( "create" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Person>> Create([FromBody] Person model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await PersonRepository.Create( model );
 
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
        public async Task<ActionResult<Person>> Update( [FromBody] Person model)
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if ( model.Id <= 0 )
                return NotFound( new { message = "Pessoa não encontrada" } );

            try
            {
                await PersonRepository.Update( model );

                return model;
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o cadastro" } );

            }
        }

        [HttpDelete]
        [Route( "delete" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Person>> Delete( int id )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if ( id <= 0)
                return NotFound( new { message = "Pessoa não encontrada" } );

            try
            {
                
                return await PersonRepository.Delete( id ); 
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível criar o cadastro" } );

            }
        }

    }
}
