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

    public class SessionsController : Controller
    {
        private readonly DBAspirantContext _context;

        public SessionsController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var dBAspirantContext = _context.Sessions.Include(s => s.Group);
            return View(await dBAspirantContext.ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessions = await _context.Sessions
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessions == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Schedules1", new { id = sessions.Id });
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,SessionName")] Sessions sessions)
        {
            var elements = _context.Sessions.ToArray();
            foreach (var c in elements)
            {
                if ((sessions.SessionName == c.SessionName) && (sessions.GroupId == sessions.GroupId))
                {
                    ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
                    ModelState.AddModelError("SessionName", "Coppy Error");
                    return View(sessions);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(sessions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", sessions.GroupId);
            return View(sessions);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessions = await _context.Sessions.FindAsync(id);
            if (sessions == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", sessions.GroupId);
            return View(sessions);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,SessionName")] Sessions sessions)
        {
            var elements = _context.Sessions.ToArray();
            foreach (var c in elements)
            {
                if ((sessions.SessionName == c.SessionName) && (sessions.GroupId == sessions.GroupId))
                {
                    ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName");
                    ModelState.AddModelError("SessionName", "Coppy Error");
                    return View(sessions);
                }
            }

            if (id != sessions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionsExists(sessions.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "GroupName", sessions.GroupId);
            return View(sessions);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessions = await _context.Sessions
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessions == null)
            {
                return NotFound();
            }

            return View(sessions);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessions = await _context.Sessions.FindAsync(id);
            _context.Sessions.Remove(sessions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionsExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
