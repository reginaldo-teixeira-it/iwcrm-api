using IWCRM.API.Data;
using IWCRM.API.Data.Repo;
using IWCRM.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace IWCRM.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route( "v1/address" )]

    public class AddresController : Controller
    {

        [HttpGet]
        [Route( "get-all" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<List<Address>>> GetAll()
        {
            var result = AddressRepository.GetAll().Result;
            return result;
        }

        [HttpGet]
        [Route( "get-byid" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Address>> GetById( int id )
        {
            return await AddressRepository.GetById( id );
        }

        [HttpPost]
        [Route( "create" )]
        [Authorize( Roles = "Administrator" )]
        public async Task<ActionResult<Address>> Create( [FromBody] Address model )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                await AddressRepository.Create( model );

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
        public async Task<ActionResult<Address>> Update([FromBody] Address model )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if ( model.Id <= 0)
                return NotFound( new { message = "Endereço não encontrada" } );

            try
            {
                await AddressRepository.Update( model );
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
        public async Task<ActionResult<Address>> Delete( int id )
        {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            // Verifica se o ID informado é o mesmo do modelo
            if (id <= 0)
                return NotFound( new { message = "Endereço não encontrado" } );

            try
            {
                return await AddressRepository.Delete( id );
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível remover o cadastro" } );

            }
        }

    }
}
