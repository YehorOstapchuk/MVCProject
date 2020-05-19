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

    public class SchedulesController : Controller
    {
        private readonly DBAspirantContext _context;

        public SchedulesController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index(int? id, string? name, int? sessionID)
        {
            if (sessionID == -1)
            {
                if (id == null) return RedirectToAction("Groups", "Index");
                ViewBag.GroupID = id;
                ViewBag.GroupName = name;
                var session = _context.Sessions.Where(b => b.GroupId == id).FirstOrDefault();
                if (session == null)
                {
                    var examByGroup = _context.Schedule.Where(b => b.SessionId == -1).Include(b => b.Exam);
                    // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
                    return View(await examByGroup.ToListAsync());
                }
                else
                {
                    var examByGroup = _context.Schedule.Where(b => b.SessionId == session.Id).Include(b => b.Exam);
                    // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
                    return View(await examByGroup.ToListAsync());
                }

            }
            else
            {
                ViewBag.GroupID = id;
                ViewBag.GroupName = name;
                var examByGroup = _context.Schedule.Where(b => b.SessionId == sessionID).Include(b => b.Exam);
                // var dBAspirantContext = _context.Aspirant.Include(a => a.GroupID);
                return View(await examByGroup.ToListAsync());
            }
        }

        // GET: Schedules/Details/5
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

        // GET: Schedules/Create
        public IActionResult Create(int sessionID, int groupID, string groupName)
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id");
            ViewBag.GroupID = groupID;
            ViewBag.GroupName = groupName;
            ViewBag.SessionID = sessionID;
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sessionId, int groupID, string groupName, [Bind("Id,ExamId")] Schedule schedule)
        {
            //ViewBag.GroupID = groupID;
            schedule.SessionId = sessionId;
          //  var examByGroup = _context.Schedule.Where(b => b.SessionId == sessionID).Include(b => b.Exam);
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                 return RedirectToAction("Index", "Schedules", new { id = groupID, name = groupName, sessionID = sessionId });
              //  return View(await examByGroup.ToListAsync());
            }
             ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", schedule.ExamId);
            // ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            //return View(schedule);
            //return View(await examByGroup.ToListAsync());
            return RedirectToAction("Index", "Schedules", new { id = groupID, name = groupName, sessionID = sessionId });
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", schedule.ExamId);
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,ExamId")] Schedule schedule)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", schedule.ExamId);
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", schedule.SessionId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
            //return RedirectToAction("Index", "Schedules", new { id = @ViewBag.GroupID, name = @ViewBag.GroupName });
             return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.Id == id);
        }
    }
}
