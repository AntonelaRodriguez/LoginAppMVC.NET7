// Se importa el espacio de nombres de MVC de ASP.NET Core, que proporciona las clases y funciones necesarias para crear controladores y vistas web.
using Microsoft.AspNetCore.Mvc;

using LoginApp.Data;
using LoginApp.Models;
using Microsoft.EntityFrameworkCore;
using LoginApp.ViewModels;

// Se importan los espacios de nombres necesarios para trabajar con autenticación y autorización en ASP.NET Core, incluyendo la clase ClaimsPrincipal,
// que representa la identidad del usuario, y CookieAuthenticationDefaults, que proporciona constantes para la autenticación basada en cookies.
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LoginApp.Controllers
{
    public class AccessController : Controller // Se define una clase de controlador llamada AccessController, que hereda de la clase Controller 
    {
        private readonly AppDbContext _appDbContext;

        public AccessController(AppDbContext appDbContext) // En el constructor, se inyecta una instancia de AppDbContext, que se utilizará para acceder a la base de datos.
        {
            _appDbContext = appDbContext;
        }

        // Se define un método de acción HTTP GET llamado SignUp, que devuelve una vista para que los usuarios se registren en la aplicación.
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // Este método SignUp maneja la solicitud POST enviada cuando un usuario intenta registrarse en la aplicación. 
        [HttpPost]
        public async Task<IActionResult> SignUp(UserVM modelo)
        {
            // Se verifica si las contraseñas coinciden
            if (modelo.Password != modelo.ConfirmPassword)
            {
                ViewData["Mensaje"] = "Las contraseña no coinciden xD";
                return View();
            }

            // Se crea un nuevo usuario utilizando los datos del modelo recibido
            User user = new User()
            {
                FullName = modelo.FullName,
                Email = modelo.Email,
                Password = modelo.Password,
            };

            // Se agrega el nuevo usuario a la base de datos y se guarda
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            // Si el usuario se creó correctamente, redirecciona al usuario al inicio de sesión
            if (user.Id != 0) return RedirectToAction("Login", "Access");

            // Si hubo un error al crear el usuario, muestra un mensaje de error
            ViewData["Mensaje"] = "Las crear el usuario, error fatal";
            return View();
        }

        //Este método maneja solicitudes GET y devuelve una vista para que los usuarios inicien sesión en la aplicación.
        //Si el usuario ya está autenticado, se lo redirige a la página de inicio.
        [HttpGet]
        public IActionResult LogIn()
        {
            // Si el usuario ya está autenticado, se redirecciona a la página de inicio
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");

            // Si no, se muestra la vista de inicio de sesión
            return View();
        }

        // Este método maneja solicitudes POST enviadas cuando los usuarios intentan iniciar sesión.
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInVM modelo)
        {
            // Se busca un usuario en la base de datos con el correo electrónico y la contraseña proporcionados.
            User? user_found = await _appDbContext.Users
                 .Where(u =>
                 u.Email == modelo.Email &&
                 u.Password == modelo.Password).FirstOrDefaultAsync();

            // Si no se encuentra el usuario, se muestra un mensaje de error
            if (user_found == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias xD";
                return View();
            }

            // Se crea una lista de claims (reclamaciones) para el usuario
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user_found.FullName)
            };

            // Se crea una identidad de claims para el usuario
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Se configuran las propiedades de autenticación
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            // Se inicia sesión con el esquema de autenticación de cookies
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            // Se redirige al usuario a la página de inicio después de iniciar sesión
            return RedirectToAction("Index", "Home");
        }
    }
}