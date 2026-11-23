using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    [AllowAnonymous]
    public class AccesoDenegadoModel : PageModel
    {
        public string Rol => User.FindFirst("RolNombre")?.Value
            ?? User.FindFirst(ClaimTypes.Role)?.Value
            ?? "sin sesión";

        public string? Pagina => HttpContext.Request.Query["ReturnUrl"].ToString();
    }
}
