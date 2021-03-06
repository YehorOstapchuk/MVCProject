﻿using System;
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
//using Microsoft.Office.Interop.Word;
////using System.Drawing;
//using Xceed.Words.NET;
//using GemBox.Document;
using SautinSoft.Document;
using SautinSoft.Document.Tables;
using SautinSoft.Document.Drawing;
//using GemBox.Document.Tables;


//using DocumentFormat.OpenXml.InkML;
//using SautinSoft.Document;
//using SautinSoft.Document;
//using DocumentFormat.OpenXml.Drawing;

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
                                         Cathedras newcath = null;
                                        bool flagg = false;
                                        string cathName = row.Cell(1).Value.ToString();
                                         var v = _context.Cathedras.ToArray();
                                         foreach(Cathedras cath in v)
                                         {
                                           string cath2Name = cath.CathedraName.ToString();
                                           if (cath2Name.Contains(cathName)) { flagg = true; newcath = cath; break; }
                                         }
                                     /*   string cathName = row.Cell(1).Value.ToString();
                                        var v = (from cath in _context.Cathedras  //.Where(k => k.FacultyId == newfac.Id)
                                                 where cath.CathedraName.Contains(cathName)
                                                select cath).ToList();
                                        if (v.Count>0)
                                        {
                                            newcath = v[0];
                                        } */
                                        if(!flagg)
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
                                        string grpName = row.Cell(2).Value.ToString();
                                        var b = (from grp in _context.Group
                                                 where grp.GroupName.Contains(grpName)
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
                                            await _context.SaveChangesAsync();
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








        public ActionResult Export_Doc()
        {
            DocumentCore dc = new DocumentCore();
            Section section = new Section(dc);
            dc.Sections.Add(section);
            section.PageSetup.PaperType = PaperType.A3;
            Table table = new Table(dc);
            bool flag = true;
            foreach (var group in _context.Group.Include(g => g.Cathedra))
            {
                var students = from s in _context.Aspirant
                               where s.GroupId == @group.Id
                               select s;
                foreach (var student in students)
                {

                    if (flag)
                    {
                        TableRow row = new TableRow(dc);


                        for (int i = 0; i < 6; i++)
                        {
                            TableCell cell = new TableCell(dc);
                            row.Cells.Add(cell);
                            Run run = new Run(dc);
                            cell.CellFormat.Borders.SetBorders(MultipleBorderTypes.Outside, BorderStyle.Outset, Color.Black, 1.0);

                            cell.CellFormat.BackgroundColor = Color.Yellow;

                            if (i == 0) run = new Run(dc, "Кафедра");
                            if (i == 1) run = new Run(dc, "Група");
                            if (i == 2) run = new Run(dc, "Ім'я");
                            if (i == 3) run = new Run(dc, "Прізвище");
                            if (i == 4) run = new Run(dc, "Форма навчання");
                            if (i == 5) run = new Run(dc, "День народження");
                            flag = false;
                            cell.Blocks.Content.Replace(run.Content);
                        }

                        table.Rows.Add(row);
                    }
                    TableRow rownew = new TableRow(dc);

                    for (int i = 0; i < 6; i++)
                    {
                        TableCell cellnew = new TableCell(dc);
                        rownew.Cells.Add(cellnew);
                        cellnew.CellFormat.Borders.SetBorders(MultipleBorderTypes.Outside, BorderStyle.Outset, Color.Black, 1.0);
                        Run runnew = new Run(dc);
                        if (i == 0) runnew = new Run(dc, group.Cathedra.CathedraName.ToString());
                        if (i == 1) runnew = new Run(dc, group.GroupName.ToString());
                        if (i == 2) runnew = new Run(dc, student.Name.ToString());
                        if (i == 3) runnew = new Run(dc, student.Surname.ToString());
                        string type;
                        if (group.TypeOfStudying == 1) type = "Очна";
                        else type = "Заочна";
                        if (i == 4) runnew = new Run(dc, type);
                        if (i == 5) runnew = new Run(dc, student.BirthDay.Value.ToShortDateString());
                        cellnew.Blocks.Content.Replace(runnew.Content);
                    }

                    table.Rows.Add(rownew);
                }
            }
            dc.Content.Start.Insert(table.Content);
            using (MemoryStream docxStream = new MemoryStream())
            {

                dc.Save(docxStream, new DocxSaveOptions());
                docxStream.Flush();

                return new FileContentResult(docxStream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.docx"
                };
            }
            //return RedirectToAction("Index", "Home");
        }








/*

        public ActionResult Export_Doc()
        {

            string filePath = @"C:\Users\Егор\Desktop\test.docx";
            using (var doc = DocX.Create(filePath))

            {
                var categories = _context.Faculties.ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in categories)
                {

                    int count = 0;
                    var facultis = _context.Faculties.Where(j => j.Id == c.Id).ToArray();
                    foreach (var k in facultis)
                    {
                        var cathedras = _context.Cathedras.Where(z => z.FacultyId == k.Id).ToArray();
                        foreach (var v in cathedras)
                        {
                            var groups = _context.Group.Where(z => z.CathedraId == v.Id).ToArray();
                            foreach (var b in groups)
                            {
                                var students = _context.Aspirant.Where(z => z.GroupId == b.Id);
                                foreach (var n in students)
                                {
                                    count++;
                                }
                            }
                        }
                    }


                    Table t = doc.AddTable(count, 6);


                    foreach (Row row in t.Rows)
                    {
                        if (row == t.Rows[0])
                        {
                            row.Cells[0].Paragraphs.First().Append("Кафедра");
                            row.Cells[1].Paragraphs.First().Append("Група");
                            row.Cells[2].Paragraphs.First().Append("Тип навчання");
                            row.Cells[3].Paragraphs.First().Append("Ім'я");
                            row.Cells[4].Paragraphs.First().Append("Прізвище");
                            row.Cells[5].Paragraphs.First().Append("Дата народження");
                        }

                        else
                        {

                            var cathedra = _context.Cathedras.Where(v => v.Faculty == c).ToList();
                            int p = 0;
                            //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                            for (int i = 0; i < cathedra.Count; i++)
                            {

                                var group = _context.Group.Where(b => b.Cathedra == cathedra[i]).ToList();
                                for (int j = 0; j < group.Count; j++)
                                {
                                    var aspirant = _context.Aspirant.Where(n => n.Group == group[j]).ToList();
                                    for (int k = 0; k < aspirant.Count; k++)
                                    {
                                        row.Cells[0].Paragraphs.First().Append(cathedra[i].CathedraName.ToString());
                                        row.Cells[1].Paragraphs.First().Append(group[j].GroupName.ToString());
                                        row.Cells[2].Paragraphs.First().Append(group[j].TypeOfStudying.ToString());
                                        row.Cells[3].Paragraphs.First().Append(aspirant[k].Name.ToString());
                                        row.Cells[4].Paragraphs.First().Append(aspirant[k].Surname.ToString());
                                        row.Cells[5].Paragraphs.First().Append(aspirant[k].BirthDay.ToString());
                                        p++;
                                    }
                                }
                            }

                        }
                    }

                }
                using (var stream = new MemoryStream())
                {
                    // doc.Save();
                    //app.Documents.Open(stream);
                    //doc.SaveAs(stream);
                    doc.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.docx"
                    };
                }
            }
        }





    */






    }
}
