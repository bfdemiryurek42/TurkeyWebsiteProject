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
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cities
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cities.Include(c => c.Food).Include(c => c.Territory).OrderBy(c=>c.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cities/Details/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Food)
                .Include(c => c.Territory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.Foods.OrderBy(f=>f.Name), "Id", "Name");
            ViewData["TerritoryId"] = new SelectList(_context.Territories.OrderBy(t => t.Name), "Id", "Name");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Create([Bind("Id,TerritoryId,FoodId,Name,Description")] City city,IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    var filePath = Path.GetTempFileName();
                    var fileName = Guid.NewGuid() + "-" + Image.FileName;
                    var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\Cities\\" + fileName;

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    city.Image = fileName;
                }
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Image", city.FoodId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "Id", "Name", city.TerritoryId);
            return View(city);
        }

        // GET: Cities/Edit/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["FoodId"] = new SelectList(_context.Foods.OrderBy(f => f.Name), "Id", "Name", city.FoodId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories.OrderBy(t => t.Name), "Id", "Name", city.TerritoryId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TerritoryId,FoodId,Name,Description")] City city,IFormFile Image, string CurrentImage)
        {
            if (id != city.Id)
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
                        var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\Cities\\" + fileName;

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }
                        city.Image = fileName;
                    }
                    else
                    {
                        city.Image = CurrentImage;
                    }
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Image", city.FoodId);
            ViewData["TerritoryId"] = new SelectList(_context.Territories, "Id", "Name", city.TerritoryId);
            return View(city);
        }

        // GET: Cities/Delete/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Food)
                .Include(c => c.Territory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
