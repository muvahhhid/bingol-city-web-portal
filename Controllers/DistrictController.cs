using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCityProject.Data;
using MyCityProject.Models;

namespace MyCityProject.Controllers
{
    public class DistrictController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistrictController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Public page: everyone can view the district list
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var districts = await _context.Districts
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();

            if (!districts.Any())
            {
                ViewBag.Message = "No district data is available yet.";
            }

            return View(districts);
        }

        // Admin only: create district page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Admin only: save new district
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(District model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Districts.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Admin only: edit district page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // Admin only: save district changes
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(District model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var districtExists = await _context.Districts.AnyAsync(d => d.Id == model.Id);

            if (!districtExists)
            {
                return NotFound();
            }

            _context.Districts.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Admin only: delete confirmation page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var district = await _context.Districts
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // Admin only: delete district after confirmation
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}