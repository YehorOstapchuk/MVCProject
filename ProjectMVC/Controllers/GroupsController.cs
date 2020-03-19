using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMVC;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;

namespace ProjectMVC.Controllers
{
    public class GroupsController : Controller
    {
        private readonly DBAspirantContext _context;

        public GroupsController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var dBAspirantContext = _context.Group.Include(ViewBag => ViewBag.Cathedra);
            return View(await dBAspirantContext.ToListAsync());
        }

        // GET: Groups/Details/5
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
            return RedirectToAction("Index", "Aspirants", new { id = group.Id, name = group.GroupName }) ;
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

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "CathedraName");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupName,TypeOfStudying,CathedraId")] Group @group)
        {
            if (ModelState.IsValid)
            {
                //ModelState.AddModelError("GroupName", "Error");
                //return View(@group);
                _context.Add(@group);
                //@group.GroupName
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "CathedraName", @group.CathedraId);
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "CathedraName", @group.CathedraId);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupName,TypeOfStudying,CathedraId")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CathedraId"] = new SelectList(_context.Cathedras, "Id", "CathedraName", @group.CathedraId);
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Group.FindAsync(id);
            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Group newgrp;
                                var c = (from grp in _context.Group
                                         where grp.GroupName.Contains(worksheet.Name)
                                         select grp).ToList();
                                if (c.Count > 0)
                                {
                                    newgrp = c[0];
                                }
                                else
                                {
                                    newgrp = new Group();
                                    newgrp.GroupName = worksheet.Name;
                                   // newgrp.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Group.Add(newgrp);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Aspirant aspirant = new Aspirant();
                                        aspirant.Name = row.Cell(1).Value.ToString();
                                        aspirant.Surname = row.Cell(2).Value.ToString();
                                        aspirant.BirthDay = (DateTime) row.Cell(3).Value;
                                        // book.Info = row.Cell(6).Value.ToString();
                                        aspirant.Group = newgrp;
                                        aspirant.GroupId = newgrp.Id;
                                        bool flag = true;
                                        var aspirants = _context.Aspirant.Where(n => n.GroupId == newgrp.Id).ToArray();
                                        foreach (var asp in aspirants)
                                        {
                                            if ((asp.Name == aspirant.Name) && (asp.Surname == aspirant.Surname) && (asp.BirthDay == aspirant.BirthDay)) { flag = false; break; }
                                        }
                                        if (flag)
                                        {
                                            _context.Aspirant.Add(aspirant);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var categories = _context.Group.Include("Aspirant").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in categories)
                {
                    var worksheet = workbook.Worksheets.Add(c.GroupName);

                    worksheet.Cell("A1").Value = "Ім'я";
                    worksheet.Cell("B1").Value = "Прізвище";
                    worksheet.Cell("C1").Value = "Дата народження";
                    worksheet.Column(3).AdjustToContents();
                   
                    worksheet.Row(1).Style.Font.Bold = true;
                    var aspirant = c.Aspirant.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < aspirant.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = aspirant[i].Name;
                        worksheet.Cell(i + 2, 2).Value = aspirant[i].Surname;
                        worksheet.Cell(i + 2, 3).Value = (DateTime) aspirant[i].BirthDay;

                       

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
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
