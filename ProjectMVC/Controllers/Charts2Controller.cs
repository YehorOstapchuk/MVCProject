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
    public class Charts2Controller : ControllerBase
    {
        public readonly DBAspirantContext _context;

        public Charts2Controller(DBAspirantContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var groups = _context.Faculties.Include(b => b.Cathedras).ToList();
            List<object> catAspirant = new List<object>();
            catAspirant.Add(new[] { "Факультет", "Кількість кафедр" });
            foreach (var c in groups)
            {
                catAspirant.Add(new object[] { c.FacultyName, c.Cathedras.Count() });

            }
            return new JsonResult(catAspirant);
        }
    }
}