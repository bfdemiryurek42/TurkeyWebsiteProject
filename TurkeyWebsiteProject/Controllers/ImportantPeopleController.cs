using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TurkeyWebsiteProject.Data;
using TurkeyWebsiteProject.Models;

namespace TurkeyWebsiteProject.Controllers
{
    public class ImportantPeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImportantPeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImportantPeople
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ImportantPeople.Include(i => i.City).OrderBy(Ip => Ip.FirstName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ImportantPeople/Details/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantPerson = await _context.ImportantPeople
                .Include(i => i.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importantPerson == null)
            {
                return NotFound();
            }

            return View(importantPerson);
        }

        // GET: ImportantPeople/Create
        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities.OrderBy(c=>c.Name), "Id", "Name");
            return View();
        }

        // POST: ImportantPeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Create([Bind("Id,CityId,FirstName,LastName,DateOfBirth,DateOfDeath")] ImportantPerson importantPerson, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    var filePath = Path.GetTempFileName();
                    var fileName = Guid.NewGuid() + "-" + Image.FileName;
                    var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\ImportantPeople\\" + fileName;

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    importantPerson.Image = fileName;
                }
                _context.Add(importantPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Image", importantPerson.CityId);
            return View(importantPerson);
        }

        // GET: ImportantPeople/Edit/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantPerson = await _context.ImportantPeople.FindAsync(id);
            if (importantPerson == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities.OrderBy(c => c.Name), "Id", "Name", importantPerson.CityId);
            return View(importantPerson);
        }

        // POST: ImportantPeople/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityId,FirstName,LastName,DateOfBirth,DateOfDeath")] ImportantPerson importantPerson, IFormFile Image,string CurrentImage)
        {
            if (id != importantPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null)
                    {
                        var filePath = Path.GetTempFileName();
                        var fileName = Guid.NewGuid() + "-" + Image.FileName;
                        var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\ImportantPeople\\" + fileName;

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }
                        importantPerson.Image = fileName;
                    }
                    else
                    {
                        importantPerson.Image = CurrentImage;
                    }
                    _context.Update(importantPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportantPersonExists(importantPerson.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Image", importantPerson.CityId);
            return View(importantPerson);
        }

        // GET: ImportantPeople/Delete/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantPerson = await _context.ImportantPeople
                .Include(i => i.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importantPerson == null)
            {
                return NotFound();
            }

            return View(importantPerson);
        }

        // POST: ImportantPeople/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var importantPerson = await _context.ImportantPeople.FindAsync(id);
            _context.ImportantPeople.Remove(importantPerson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImportantPersonExists(int id)
        {
            return _context.ImportantPeople.Any(e => e.Id == id);
        }
    }
}
