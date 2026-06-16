using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCityProject.Controllers
{
    public class CityController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
