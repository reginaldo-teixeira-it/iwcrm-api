using IWCRM.API.Data;
using IWCRM.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IWCRM.API.Controllers
{
    [ApiController]
    [Route( "v1" )]
    public class PersonController : Controller
    {
        [HttpGet]
        [Route( "persons" )]
        public async Task<ActionResult<List<Person>>> GetAll( [FromServices] DataContext context )
        {
          return await context.Person.AsNoTracking().ToListAsync();
        }

        [HttpGet]
        [Route( "persons/{id:int}" )]
        public async Task<ActionResult<Person>> GetById( [FromServices] DataContext context, int id )
        {
            return await context.Person.AsNoTracking().FirstOrDefaultAsync( x => x.Id == id );
        }

    }
}
