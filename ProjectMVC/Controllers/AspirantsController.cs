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
    public class AspirantsController : Controller
    {
        private readonly DBAspirantContext _context;

        public AspirantsController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Aspirants
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Groups", "Index");
            ViewBag.GroupID = id;
            ViewBag.GroupName = name;
            var aspirantsByGroup = _context.Aspirant.Where(b => b.GroupId == id).Include(b => b.Group);
           // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
            return View(await aspirantsByGroup.ToListAsync());
        }

        // GET: Aspirants/Details/5
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

        // GET: Aspirants/Create
        public IActionResult Create(int groupID)
        {
            ViewBag.GroupID = groupID;
            ViewBag.GroupName = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName;
           // ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
            return View();
        }

        // POST: Aspirants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int groupID ,[Bind("Id,Name,Surname,BirthDay")] Aspirant aspirant)
        {
          //  if (ModelState.ContainsKey("{Name}"))
            //    ModelState["{Name}"].Errors.Clear();

          //  DateTime time_now = DateTime.Now;
          //  DateTime d2 = aspirant.BirthDay.Value;
          //  if (DateTime.Compare(time_now, d2) <= 0) { ModelState.AddModelError("BirthDay", "BirthDay Error"); return View(aspirant); }




            aspirant.GroupId = groupID;
            if (ModelState.IsValid)
           {

                var elements = _context.Aspirant.ToArray();
                foreach (var c in elements)
                {
                    //if ((aspirant.Name == c.Name) && (aspirant.Surname == c.Surname) && (aspirant.BirthDay == c.BirthDay) )
                    if ((aspirant.Name == c.Name) && (aspirant.Surname == c.Surname))
                    {
                        ViewData["GroupId"] = groupID;
                        ModelState.AddModelError("Name", "Coppy Error");
                        return View(aspirant);
                    }
                }


              //  if (_context.Aspirant.Where(c => (c.Name == aspirant.Name)&&(c.Surname == aspirant.Surname)).Count() != 0 )
               // {
               //     ModelState.AddModelError("Name", "Coppy Error");
               //     return View(aspirant);
              //  }
                _context.Add(aspirant);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Aspirants", new { id = groupID, name = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName });
            }
            //ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
           // return RedirectToAction("Index", "Aspirants", new { id = groupID, name = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName });
             return View(aspirant);
        }

        // GET: Aspirants/Edit/5
        public async Task<IActionResult> Edit(int? id, int groupID)
        {

            ViewBag.GroupID = groupID;
            if (id == null)
            {
                return NotFound();
            }

            var aspirant = await _context.Aspirant.FindAsync(id);
            if (aspirant == null)
            {
                return NotFound();
            }
           // ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
            return View(aspirant);
        }

        // POST: Aspirants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int groupID, [Bind("Id,Name,Surname,BirthDay")] Aspirant aspirant)
        {
         //   DateTime time_now = DateTime.Now;
            // DateTime d1 = new DateTime(2020, 1, 20, 6, 20, 40);
          //  DateTime d2 = aspirant.BirthDay.Value;
          //  if (DateTime.Compare(time_now, d2) <= 0) { ModelState.AddModelError("BirthDay", "BirthDay Error"); return View(aspirant); }

            aspirant.GroupId = groupID;
            if (id != aspirant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return RedirectToAction("Index", "Aspirants", new { id = groupID, name = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName });
             //  return RedirectToAction(nameof(Index));
              }
            return RedirectToAction("Index", "Aspirants", new { id = groupID, name = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName });
            // ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", aspirant.GroupId);
            // return View(aspirant);
        }

        // GET: Aspirants/Delete/5
        public async Task<IActionResult> Delete(int? id, int groupID)
        {
            ViewBag.GroupID = groupID;
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

        // POST: Aspirants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int groupID)
        {
            var aspirant = await _context.Aspirant.FindAsync(id);
            _context.Aspirant.Remove(aspirant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Aspirants", new { id = groupID, name = _context.Group.Where(c => c.Id == groupID).FirstOrDefault().GroupName });
            //return RedirectToAction(nameof(Index));
        }

        private bool AspirantExists(int id)
        {
            return _context.Aspirant.Any(e => e.Id == id);
        }



        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckBirth(DateTime BirthDay)
        {

            DateTime time_now = DateTime.Now;
            // DateTime d1 = new DateTime(2020, 1, 20, 6, 20, 40);
            // DateTime d2 = aspirant.BirthDay.Value;
            if (DateTime.Compare(time_now, BirthDay) <= 0)
            {
                return Json(false);
            }
            return Json(true);
        }
    }


}
