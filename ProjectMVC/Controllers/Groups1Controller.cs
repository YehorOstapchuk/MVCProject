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

    public class Groups1Controller : Controller
    {
        private readonly DBAspirantContext _context;

        public Groups1Controller(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Groups1
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Home", "Index");
            ViewBag.CathedraID = id;
            ViewBag.CathedraName = name;
            var GroupByCathedras = _context.Group.Where(b => b.CathedraId == id).Include(b => b.Cathedra);
            // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
            return View(await GroupByCathedras.ToListAsync());
        }

        // GET: Groups1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(ViewBag => ViewBag.Cathedra)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            //return View(@group);
            return RedirectToAction("Index", "Aspirants", new { id = group.Id, name = group.GroupName });
        }



        public async Task<IActionResult> Schedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(ViewBag => ViewBag.Cathedra)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            //return View(@group);
            int i = -1;
            return RedirectToAction("Index", "Schedules", new { id = group.Id, name = group.GroupName, sessionID = i });
        }


        // GET: Groups1/Create
        public IActionResult Create(int cathedraID)
        {
            ViewBag.CathedraID = cathedraID;
            ViewBag.GroupName = _context.Cathedras.Where(c => c.Id == cathedraID).FirstOrDefault().CathedraName;
            // ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
            return View();
        }

        // POST: Groups1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int cathedraID, [Bind("Id,GroupName,TypeOfStudying")] Group @group)
        {
            @group.CathedraId = cathedraID;
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                //  return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Groups1", new { id = cathedraID, name = _context.Cathedras.Where(c => c.Id == cathedraID).FirstOrDefault().CathedraName });
            }
            //ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "Id", @group.CathedraId);
            //  return View(@group);
            return RedirectToAction("Index", "Groups1", new { id = cathedraID, name = _context.Cathedras.Where(c => c.Id == cathedraID).FirstOrDefault().CathedraName });
        }

        // GET: Groups1/Edit/5
        public async Task<IActionResult> Edit(int? id, int cathedraID)
        {
            ViewBag.CathedraID = cathedraID;
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
           // ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "Id", @group.CathedraId);
            return View(@group);
        }

        // POST: Groups1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int cathedraID, [Bind("Id,GroupName,TypeOfStudying")] Group @group)
        {
            @group.CathedraId = cathedraID;
            if (id != @group.Id)
            {
                return NotFound();
            }

          //  if (ModelState.IsValid)
           // {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return RedirectToAction("Index", "Groups1", new { id = cathedraID, name = _context.Cathedras.Where(c => c.Id == cathedraID).FirstOrDefault().CathedraName });
            //    return RedirectToAction(nameof(Index));
            // }
            //  ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "Id", @group.CathedraId);
            // return View(@group);
        }

        // GET: Groups1/Delete/5
        public async Task<IActionResult> Delete(int? id, int cathedraID)
        {
            ViewBag.CathedraID = cathedraID;
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(ViewBag => ViewBag.Cathedra)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int cathedraID)
        {
            var @group = await _context.Group.FindAsync(id);
            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Groups1", new { id = cathedraID, name = _context.Cathedras.Where(c => c.Id == cathedraID).FirstOrDefault().CathedraName });
            //return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckGroup(string groupName)
        {
            var elements = _context.Group.ToArray();
            foreach (var c in elements)
            {
                if (groupName == c.GroupName)
                {
                    return Json(false);
                }
            }
            return Json(true);
        }
    }
}
