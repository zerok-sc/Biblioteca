using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly IAutenticacion autenticacion;

        public LogoutModel(IAutenticacion autenticacion)
        {
            this.autenticacion = autenticacion;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var valor = User.FindFirst("SesionId")?.Value;
            if (int.TryParse(valor, out var sesionId) && sesionId > 0)
            {
                try
                {
                    await autenticacion.CerrarSesion(sesionId);
                }
                catch
                {
                    // El cierre local no debe impedir salir del sistema.
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await OnPostAsync();
        }
    }
}
