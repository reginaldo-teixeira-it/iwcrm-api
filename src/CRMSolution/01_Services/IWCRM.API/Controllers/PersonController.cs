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

            try
            {
                var persons = await context.Person.AsNoTracking().ToListAsync();
                return persons;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
