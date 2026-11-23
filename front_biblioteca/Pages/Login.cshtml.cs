using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IAutenticacion autenticacion;

        [BindProperty] public string Usuario { get; set; } = "";
        [BindProperty] public string Clave { get; set; } = "";
        public string? Error { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public LoginModel(IAutenticacion autenticacion)
        {
            this.autenticacion = autenticacion;
        }

        public void OnGet(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                ReturnUrl = returnUrl;
        }

        public IActionResult OnPostLimpiar()
        {
            ModelState.Clear();
            Usuario = "";
            Clave = "";
            Error = null;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            var datos = await autenticacion.IniciarSesion(
                Usuario,
                Clave,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString());
            if (datos is null)
            {
                Error = "Usuario o clave incorrectos.";
                Clave = "";
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, datos.nombre_usuario),
                new(ClaimTypes.NameIdentifier, datos.id.ToString()),
                new(ClaimTypes.Email, datos.correo),
                new(ClaimTypes.Role, datos.rol_codigo),
                new("RolNombre", datos.rol_nombre),
                new("PersonaNombre", datos.persona_nombre),
                new("PersonaId", datos.persona_id.ToString()),
                new("ImagenTipo", datos.imagen_tipo ?? "image/png"),
                new("SesionId", datos.sesion_id.ToString())
            };

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identidad));

            var destino = returnUrl ?? ReturnUrl;
            if (!string.IsNullOrEmpty(destino) && Url.IsLocalUrl(destino))
                return LocalRedirect(destino);

            return RedirectToPage("/Index");
        }
    }
}
