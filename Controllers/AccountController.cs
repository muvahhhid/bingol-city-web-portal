using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCityProject.Models;

namespace MyCityProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        // Admin login page
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Admin login form submit
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email?.Trim();
            var password = model.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please try again.");
            return View(model);
        }

        // Logout confirmation page
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        // Logout after confirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Logout directly from navbar form/button if used
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutFromNavbar()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}