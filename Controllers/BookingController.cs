using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;   // важно!
using MyCityProject.Data;
using MyCityProject.Models;

namespace MyCityProject.Controllers
{
    public class BookingController : Controller

    {
        private readonly ApplicationDbContext _context;

        // В конструктор вколем контекст
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Если нужно использовать асинхронность — ToListAsync()
            var bookings = _context.Bookings.ToList();
            return View(bookings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges(); // метод доступен благодаря using Microsoft.EntityFrameworkCore;
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        public IActionResult Edit(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        public IActionResult Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Bookings.Update(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
