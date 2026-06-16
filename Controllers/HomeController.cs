using Microsoft.AspNetCore.Mvc;

namespace MyCityProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Главная страница приветствия
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
