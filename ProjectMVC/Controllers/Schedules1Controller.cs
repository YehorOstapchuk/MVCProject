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

    public class Schedules1Controller : Controller
    {
        private readonly DBAspirantContext _context;

        public Schedules1Controller(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Schedules1
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction("Groups", "Index");
            ViewBag.SessionID = id;
            var scheduleBysession = _context.Schedule.Where(b => b.SessionId == id).Include(b => b.Exam);
            // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
            return View(await scheduleBysession.ToListAsync());
        }

        // GET: Schedules1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Exam)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules1/Create
        public IActionResult Create(int sessionId)
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "ExamName");
            ViewBag.SessionID = sessionId;
            return View();
        }

        // POST: Schedules1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sessionId, [Bind("Id,ExamId")] Schedule schedule)
        {
            schedule.SessionId = sessionId;
          //  if (ModelState.IsValid)
           // {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Schedules1", new { id = sessionId });
         //   }
         //   ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "ExamName", schedule.ExamId);
            //ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return RedirectToAction("Index", "Schedules1", new { id = sessionId });
        }

        // GET: Schedules1/Edit/5
        public async Task<IActionResult> Edit(int? id, int sessionID)
        {
            ViewBag.SessionID = sessionID;

            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "ExamName", schedule.ExamId);
            //ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return View(schedule);
        }

        // POST: Schedules1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int sessionID, [Bind("Id,SessionId,ExamId")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return RedirectToAction("Index", "Schedules1", new { id = sessionID });
            //   return RedirectToAction(nameof(Index));
              }
            return RedirectToAction("Index", "Schedules1", new { id = sessionID });
            //  ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", schedule.ExamId);
            //ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            //  return View(schedule);
        }

        // GET: Schedules1/Delete/5
        public async Task<IActionResult> Delete(int? id, int sessionID)
        {
            ViewBag.SessionID = sessionID;
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Exam)
                .Include(s => s.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int sessionID)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Schedules1", new { id = sessionID });
            //return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.Id == id);
        }
    }
}
