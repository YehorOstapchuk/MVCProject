using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        public readonly DBAspirantContext _context;

        public ChartsController (DBAspirantContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData ()
        {
            var groups = _context.Group.Include(b => b.Aspirant).ToList();
            List<object> catAspirant = new List<object>();
            catAspirant.Add(new[] { "Група", "Кількість студентів" });
            foreach( var c in groups)
            {
                catAspirant.Add(new object[] { c.GroupName, c.Aspirant.Count() });

            }
            return new JsonResult(catAspirant);
        }
    }
}