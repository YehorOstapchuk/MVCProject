using System;
using System.IO;
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
    public class FacultiesController : Controller
    {
        private readonly DBAspirantContext _context;

        public FacultiesController(DBAspirantContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculties.ToListAsync());
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculties == null)
            {
                return NotFound();
            }

            // return View(faculties);
            return RedirectToAction("Index", "Cathedras", new { id = faculties.Id, name = faculties.FacultyName });
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FacultyName")] Faculties faculties)
        {
           /* var elements = _context.Faculties.ToArray();
            foreach (var c in elements)
            {
                if (faculties.FacultyName == c.FacultyName)
                {
                    ModelState.AddModelError("FacultyName", "Coppy Error");
                    return View(faculties);
                }
            } */

            if (ModelState.IsValid)
            {
                _context.Add(faculties);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculties);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties.FindAsync(id);
            if (faculties == null)
            {
                return NotFound();
            }
            return View(faculties);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FacultyName")] Faculties faculties)
        {
            /*var elements = _context.Faculties.ToArray();
            foreach (var c in elements)
            {
                if (faculties.FacultyName == c.FacultyName)
                {
                    ModelState.AddModelError("FacultyName", "Coppy Error");
                    return View(faculties);
                }
            }*/

            if (id != faculties.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultiesExists(faculties.Id))
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
            return View(faculties);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            int count = 0;
            var facultis = _context.Faculties.Where(c => c.Id == id).ToArray();
            foreach(var c in facultis)
            {
                var cathedras = _context.Cathedras.Where(z => z.FacultyId == c.Id).ToArray();
                foreach( var v in cathedras)
                {
                    var groups = _context.Group.Where(z => z.CathedraId == v.Id).ToArray();
                    foreach(var b in groups)
                    {
                        var students = _context.Aspirant.Where(z => z.GroupId == b.Id);
                        foreach(var n in students)
                        {
                            count++;
                        }
                    }
                }
            }

            ViewBag.Count = count;

            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculties == null)
            {
                return NotFound();
            }

            return View(faculties);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculties = await _context.Faculties.FindAsync(id);
          //  var cathedra = await _context.Cathedras.Where(s => s.FacultyId == id).AllAsync();
           // _context.Cathedras.Update(cathedra);
            _context.Faculties.Remove(faculties);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultiesExists(int id)
        {
            return _context.Faculties.Any(e => e.Id == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckFaculty(string facultyName)
        {
            var elements = _context.Faculties.ToArray();
            foreach (var c in elements)
            {
                if (facultyName == c.FacultyName)
                {
                    return Json(false);
                }
            }               
            return Json(true);
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
                                Faculties newfac;
                                var c = (from fac in _context.Faculties
                                         where fac.FacultyName.Contains(worksheet.Name)
                                         select fac).ToList();
                                if (c.Count > 0)
                                {
                                    newfac = c[0];
                                }
                                else
                                {
                                    newfac = new Faculties();
                                    newfac.FacultyName = worksheet.Name;
                                    // newgrp.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Faculties.Add(newfac);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                         Cathedras newcath;
                                       //ool flagg = false;
                                      //string cathName = row.Cell(1).Value.ToString();
                                     // var v = _context.Cathedras.ToArray();
                                     // foreach(var cath in v)
                                     // {
                                     //     if (cath.CathedraName == cathName) { flagg = true; break; }
                                     // }
                                        var v = (from cath in _context.Cathedras  //.Where(k => k.FacultyId == newfac.Id)
                                                 where cath.CathedraName.Contains(row.Cell(1).Value.ToString())
                                                select cath).ToList();
                                        if (v.Count>0)
                                        {
                                            newcath = v[0];
                                        }
                                        else
                                        {
                                            newcath = new Cathedras();
                                            newcath.CathedraName = row.Cell(1).Value.ToString();
                                            newcath.FacultyId = newfac.Id;
                                            newcath.Faculty = newfac;
                                            // newgrp.Info = "from EXCEL";
                                            //додати в контекст
                                            _context.Cathedras.Add(newcath);
                                        }

                                        Group newgrp;
                                        var b = (from grp in _context.Group
                                                 where grp.GroupName.Contains(row.Cell(2).Value.ToString())
                                                 select grp).ToList();
                                        if (b.Count > 0)
                                        {
                                            newgrp = b[0];
                                        }
                                        else
                                        {
                                            newgrp = new Group();
                                            newgrp.GroupName = row.Cell(2).Value.ToString();
                                            newgrp.TypeOfStudying = Convert.ToInt32(row.Cell(3).Value.ToString());
                                            newgrp.CathedraId = newcath.Id;
                                            newgrp.Cathedra = newcath;
                                            // newgrp.Info = "from EXCEL";
                                            //додати в контекст
                                            _context.Group.Add(newgrp);
                                        }


                                        Aspirant aspirant = new Aspirant();
                                        aspirant.Name = row.Cell(4).Value.ToString();
                                        aspirant.Surname = row.Cell(5).Value.ToString();
                                        aspirant.BirthDay = (DateTime)row.Cell(6).Value;
                                        // book.Info = row.Cell(6).Value.ToString();
                                        aspirant.Group = newgrp;
                                        aspirant.GroupId = newgrp.Id;
                                        bool flag = true;
                                        var aspirants = _context.Aspirant.Where(n => n.GroupId == newgrp.Id).ToArray();
                                        foreach(var asp in aspirants)
                                        {
                                            if (( asp.Name == aspirant.Name)&&(asp.Surname == aspirant.Surname)&&(asp.BirthDay == aspirant.BirthDay)) { flag = false; break; }
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
                var categories = _context.Faculties.ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in categories)
                {
                    var worksheet = workbook.Worksheets.Add(c.FacultyName);

                    worksheet.Cell("A1").Value = "Кафедра";
                    worksheet.Cell("B1").Value = "Група";
                    worksheet.Cell("C1").Value = "Тип навчання";
                    worksheet.Cell("D1").Value = "Ім'я";
                    worksheet.Cell("E1").Value = "Прізвище";
                    worksheet.Cell("F1").Value = "Дата народження";
                    worksheet.Column(6).AdjustToContents();

                    worksheet.Row(1).Style.Font.Bold = true;
                    var cathedra = _context.Cathedras.Where(v => v.Faculty == c ).ToList();
                    int p = 0;
                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < cathedra.Count; i++)
                    {

                        var group = _context.Group.Where(b => b.Cathedra == cathedra[i]).ToList();
                        for(int j = 0; j < group.Count; j++)
                        {
                            var aspirant = _context.Aspirant.Where(n => n.Group == group[j]).ToList();
                            for(int k = 0; k < aspirant.Count; k++)
                            {
                                worksheet.Cell(p + 2, 1).Value = cathedra[i].CathedraName;
                                worksheet.Cell(p + 2, 2).Value = group[j].GroupName;
                                worksheet.Cell(p + 2, 3).Value =(int) group[j].TypeOfStudying;
                                worksheet.Cell(p + 2, 4).Value = aspirant[k].Name;
                                worksheet.Cell(p + 2, 5).Value = aspirant[k].Surname;
                                worksheet.Cell(p + 2, 6).Value = (DateTime)aspirant[k].BirthDay;
                                p++;
                            }
                        }
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


    }
}
