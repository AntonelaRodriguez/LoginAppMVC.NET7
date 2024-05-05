// Este modelo se usa comúnmente para transferir datos entre la vista (página web) y el controlador en ASP.NET Core MVC,
// proporcionando una forma estructurada de pasar información específica de inicio de sesión al controlador para su procesamiento.

namespace LoginApp.ViewModels
{
    public class UserVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
