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
    public class Aspirants1Controller : Controller
    {
        private readonly DBAspirantContext _context;

        public Aspirants1Controller(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Aspirants1
        public async Task<IActionResult> Index()
        {
            var dBAspirantContext = _context.Aspirant.Include(a => a.Group);
            return View(await dBAspirantContext.ToListAsync());
        }

        // GET: Aspirants1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirant
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspirant == null)
            {
                return NotFound();
            }

            return View(aspirant);
        }

        // GET: Aspirants1/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
            return View();
        }

        // POST: Aspirants1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,BirthDay,GroupId")] Aspirant aspirant)
        {
            if (ModelState.IsValid)
            {
                DateTime time_now = DateTime.Now;
                DateTime d1 = new DateTime(2020, 1, 20, 6, 20, 40);
                DateTime d2 = aspirant.BirthDay.Value;
                if (DateTime.Compare(time_now, d2) <= 0) { ModelState.AddModelError("BirthDay", "BirthDay Error");
                    ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
                    return View(aspirant) ; }

                var elements = _context.Aspirant.ToArray();
                foreach (var c in elements)
                {
                    //if ((aspirant.Name == c.Name) && (aspirant.Surname == c.Surname) && (aspirant.BirthDay == c.BirthDay) )
                    if ((aspirant.Name == c.Name) && (aspirant.Surname == c.Surname))
                    {
                        ModelState.AddModelError("Name", "Coppy Error");
                        ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
                        return View(aspirant);
                    }
                }
                _context.Add(aspirant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
            return View(aspirant);
        }

        // GET: Aspirants1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirant.FindAsync(id);
            if (aspirant == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
            return View(aspirant);
        }

        // POST: Aspirants1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,BirthDay,GroupId")] Aspirant aspirant)
        {
            if (id != aspirant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                   DateTime time_now = DateTime.Now;
                 DateTime d1 = new DateTime(2020, 1, 20, 6, 20, 40);
                  DateTime d2 = aspirant.BirthDay.Value;
                  if (DateTime.Compare(time_now, d2) <= 0) { ModelState.AddModelError("BirthDay", "BirthDay Error");
                    ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
                    return View(aspirant); }



                try
                {
                    _context.Update(aspirant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspirantExists(aspirant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
            return View(aspirant);
        }

        // GET: Aspirants1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirant
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspirant == null)
            {
                return NotFound();
            }

            return View(aspirant);
        }

        // POST: Aspirants1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aspirant = await _context.Aspirant.FindAsync(id);
            _context.Aspirant.Remove(aspirant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspirantExists(int id)
        {
            return _context.Aspirant.Any(e => e.Id == id);
        }



        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckBirth(DateTime d2)
        {

            DateTime time_now = DateTime.Now;
            // DateTime d1 = new DateTime(2020, 1, 20, 6, 20, 40);
            // DateTime d2 = aspirant.BirthDay.Value;
            if (DateTime.Compare(time_now, d2) <= 0)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
