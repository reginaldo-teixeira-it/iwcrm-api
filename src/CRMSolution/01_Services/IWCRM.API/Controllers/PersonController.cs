using IWCRM.API.Data;
using IWCRM.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IWCRM.API.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<List<Person>>> GetAll( [FromServices] DataContext context )
        {
            var result = await context.Person.Include( x => x.IdAddress ).AsNoTracking().ToListAsync();
            return result;
        }


    }
}
