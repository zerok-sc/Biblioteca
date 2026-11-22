using System.Security.Claims;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages
{
    [Authorize]
    public class PerfilModel : PageModel
    {
        private readonly IAutenticacion autenticacion;

        [BindProperty] public IFormFile? FormFile { get; set; }
        public string? Imagen { get; set; }
        public string TipoImagen { get; set; } = "image/png";
        public string Nombre => User.Identity?.Name ?? "";
        public string Correo => User.FindFirst(ClaimTypes.Email)?.Value ?? "";
        public string Rol => User.FindFirst("RolNombre")?.Value
            ?? User.FindFirst(ClaimTypes.Role)?.Value ?? "";
        public int Id => int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : 0;

        public PerfilModel(IAutenticacion autenticacion)
        {
            this.autenticacion = autenticacion;
        }

        public async Task OnGetAsync()
        {
            TipoImagen = User.FindFirst("ImagenTipo")?.Value ?? "image/png";
            await CargarImagenAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TipoImagen = User.FindFirst("ImagenTipo")?.Value ?? "image/png";

            if (FormFile is null || FormFile.Length == 0)
            {
                ViewData["Mensaje"] = "Seleccione una imagen.";
                await CargarImagenAsync();
                return Page();
            }

            if (FormFile.Length > 2 * 1024 * 1024)
            {
                ViewData["Mensaje"] = "La imagen debe pesar menos de 2 MB.";
                await CargarImagenAsync();
                return Page();
            }

            try
            {
                using var memoria = new MemoryStream();
                await FormFile.CopyToAsync(memoria);
                await autenticacion.CambiarFoto(Id, memoria.ToArray(), FormFile.ContentType);
                ViewData["Mensaje"] = "Foto actualizada correctamente.";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarImagenAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCerrarAsync()
        {
            ViewData.Remove("Mensaje");
            await CargarImagenAsync();
            return Page();
        }

        private async Task CargarImagenAsync()
        {
            try
            {
                Imagen = await autenticacion.ObtenerFoto(Id);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }
    }
}
