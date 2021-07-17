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
    [Authorize(Roles = "ADMINISTRATOR")]
    public class TerritoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerritoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Territories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Territories.OrderBy(t=> t.Name).ToListAsync());
        }

        // GET: Territories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // GET: Territories/Create
        public IActionResult Create()
        {
            return View();
        }
            
        // POST: Territories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Territory territory, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    var filePath = Path.GetTempFileName();
                    var fileName = Guid.NewGuid() + "-" + Image.FileName;
                    var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\Territories\\" + fileName;

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    territory.Image = fileName;
                }
                _context.Add(territory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(territory);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories.FindAsync(id);
            if (territory == null)
            {
                return NotFound();
            }
            return View(territory);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Territory territory, IFormFile Image, string CurrentImage)
        {
            if (id != territory.Id)
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
                        var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\Territories\\" + fileName;

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }
                        territory.Image = fileName;
                    }
                    else
                    {
                        territory.Image = CurrentImage;
                    }
                    _context.Update(territory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerritoryExists(territory.Id))
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
            return View(territory);
        }

        // GET: Territories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // POST: Territories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var territory = await _context.Territories.FindAsync(id);
            _context.Territories.Remove(territory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerritoryExists(int id)
        {
            return _context.Territories.Any(e => e.Id == id);
        }
    }
}
