using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ImportantPeople.Include(i => i.City).OrderBy(Ip => Ip.FirstName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ImportantPeople/Details/5
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
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Image");
            return View();
        }

        // POST: ImportantPeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityId,FirstName,LastName,Image,DateOfBirth,DateOfDeath")] ImportantPerson importantPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(importantPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Image", importantPerson.CityId);
            return View(importantPerson);
        }

        // GET: ImportantPeople/Edit/5
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Image", importantPerson.CityId);
            return View(importantPerson);
        }

        // POST: ImportantPeople/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityId,FirstName,LastName,Image,DateOfBirth,DateOfDeath")] ImportantPerson importantPerson)
        {
            if (id != importantPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
