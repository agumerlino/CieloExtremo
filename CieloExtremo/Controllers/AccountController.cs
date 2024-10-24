using CieloExtremo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CieloExtremo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (_authService.ValidateUser(username, password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false, // La sesión no se mantendrá después de cerrar el navegador
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Productos");
            }

            ViewData["ErrorMessage"] = "Invalid login attempt.";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Cierra la sesión del usuario
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige a la página de inicio o cualquier otra página después del logout
            return RedirectToAction("Index", "Home");
        }
    }
}
