using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMVC;

namespace ProjectMVC.Controllers
{

    public class CathedrasController : Controller
    {
        //int facId;

        private readonly DBAspirantContext _context;

        public CathedrasController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Cathedras
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Home", "Index");
            ViewBag.FacultyID = id;
            ViewBag.FacultyName = name;
            var cathByfac = _context.Cathedras.Where(b => b.FacultyId == id).Include(b => b.Faculty);
            // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
            return View(await cathByfac.ToListAsync());
        }

        // GET: Cathedras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cathedras = await _context.Cathedras
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cathedras == null)
            {
                return NotFound();
            }

            //return View(cathedras);
            return RedirectToAction("Index", "Groups1", new { id = cathedras.Id, name = cathedras.CathedraName });
        }

        // GET: Cathedras/Create
        public IActionResult Create(int facultyID)
        {
            ViewBag.FacultyID = facultyID;
            ViewBag.FacultyName = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault().FacultyName;
            // ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
            return View();
        }

        // POST: Cathedras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int facultyID, [Bind("Id,CathedraName")] Cathedras cathedras)
        {
          /*  var elements = _context.Cathedras.ToArray();
            foreach (var c in elements)
            {
                if (cathedras.CathedraName == c.CathedraName)
                {
                    ModelState.AddModelError("CathedraName", "Coppy Error");
                    return View(cathedras);
                }
            } */


            cathedras.FacultyId = facultyID;
            if (ModelState.IsValid)
            {
                _context.Add(cathedras);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault().FacultyName });
           }
            return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault() });
        }

        // GET: Cathedras/Edit/5
        public async Task<IActionResult> Edit(int? id, int facultyID)
        {
            ViewBag.FacultyID =facultyID;

            if (id == null)
            {
                return NotFound();
            }

            var cathedras = await _context.Cathedras.FindAsync(id);
            if (cathedras == null)
            {
                return NotFound();
            }
           // ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Id", cathedras.FacultyId);
            return View(cathedras);
        }

        // POST: Cathedras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int facultyID, [Bind("Id,CathedraName")] Cathedras cathedras)
        {
          /*  var elements = _context.Cathedras.ToArray();
            foreach (var c in elements)
            {
                if (cathedras.CathedraName == c.CathedraName)
                {
                    ModelState.AddModelError("CathedraName", "Coppy Error");
                    return View(cathedras);
                }
            } */

            cathedras.FacultyId = facultyID;
            if (id != cathedras.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cathedras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CathedrasExists(cathedras.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // return RedirectToAction(nameof(Index));
                //    return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault() });
                return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault().FacultyName });
                   }
                // ViewData["FacultyId"] = new SelectList(_context.Faculties, "Id", "Id", cathedras.FacultyId);
                //return View(cathedras);
                return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault().FacultyName });
        }

        // GET: Cathedras/Delete/5
        public async Task<IActionResult> Delete(int? id, int facultyID)
        {
            ViewBag.FacultyID = facultyID;
            if (id == null)
            {
                return NotFound();
            }

            var cathedras = await _context.Cathedras
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cathedras == null)
            {
                return NotFound();
            }

            return View(cathedras);
        }

        // POST: Cathedras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int facultyID)
        {
           // int i = facultyID;
            var cathedras = await _context.Cathedras.FindAsync(id);
            _context.Cathedras.Remove(cathedras);
            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Cathedras", new { id = facultyID, name = _context.Faculties.Where(c => c.Id == facultyID).FirstOrDefault().FacultyName });
        }

        private bool CathedrasExists(int id)
        {
            return _context.Cathedras.Any(e => e.Id == id);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckCathedra(string cathedraName)
        {

                var elements = _context.Cathedras.ToArray();
                foreach (var c in elements)
                {
                    if (cathedraName == c.CathedraName)
                    {
                        return Json(false);
                    }
               }
            return Json(true);
        }
    }
}
