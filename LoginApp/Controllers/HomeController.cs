using LoginApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace LoginApp.Controllers
{
    // Se aplica el atributo [Authorize], lo que significa que todas las acciones de este controlador requerirán que el usuario esté autenticado para acceder a ellas. 
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            // Esta línea utiliza el método SignOutAsync del objeto HttpContext para cerrar la sesión del usuario.
            // Toma como argumento el esquema de autenticación utilizado para iniciar sesión, que en este caso es el esquema de autenticación de cookies (CookieAuthenticationDefaults.AuthenticationScheme).
            // Al llamar a este método, se elimina la identidad del usuario actual y se borran las cookies de autenticación asociadas, lo que efectivamente cierra la sesión del usuario.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Esto generalmente llevará al usuario a la página de inicio de sesión, permitiéndole iniciar sesión nuevamente si así lo desea.
            return RedirectToAction("LogIn", "Access");
        }
    }
}