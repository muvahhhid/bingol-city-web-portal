using Microsoft.AspNetCore.Mvc;
using MyCityProject.Data;
using MyCityProject.Models;
using System.Linq;

namespace MyCityProject.Controllers
{
    public class PopulationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PopulationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var populations = _context.Populations.ToList(); // Извлечение данных из базы
            return View(populations); // Передача данных в представление
        }
    }
}
