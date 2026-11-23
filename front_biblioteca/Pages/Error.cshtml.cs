using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            this.logger = logger;
        }

        public string Mensaje { get; private set; } = "No se pudo procesar la solicitud.";
        public string Referencia { get; private set; } = "";

        public void OnGet()
        {
            // El middleware ya respondio 500; mostramos la pagina con estado 200.
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            var excepcion = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (excepcion is null)
                return;

            var referencia = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            logger.LogError(excepcion, "Error {Referencia} en {Ruta}",
                referencia, HttpContext.Request.Path);

            Mensaje = "No se pudo procesar la solicitud. El error fue registrado.";
            Referencia = referencia;
        }
    }
}
