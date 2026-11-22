using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    public class ErrorModel : PageModel
    {
        public string Mensaje { get; set; } = "No se pudo procesar la solicitud.";
    }
}
