using aplicaciones_libreria.entidades;
using aplicaciones_libreria.nucleo;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages
{
    [Authorize(Roles = Codigos.RolAdmin)]
    public class CrearUsuarioModel : PageModel
    {
        private readonly IAutenticacion autenticacion;
        private readonly IPresentacion<Roles> roles;
        private readonly IPresentacion<Personas> personas;

        [BindProperty] public PedidoCrearUsuario Formulario { get; set; } = new();
        [BindProperty] public string ConfirmarClave { get; set; } = "";
        public List<Roles> ListaRoles { get; set; } = new();
        public List<Personas> ListaPersonas { get; set; } = new();
        public List<SelectListItem> OpcionesPersonas { get; private set; } = new();

        public CrearUsuarioModel(
            IAutenticacion autenticacion,
            IPresentacion<Roles> roles,
            IPresentacion<Personas> personas)
        {
            this.autenticacion = autenticacion;
            this.roles = roles;
            this.personas = personas;
        }

        public async Task OnGetAsync()
        {
            await CargarListasAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Formulario.nombre_usuario)
                || string.IsNullOrWhiteSpace(Formulario.correo)
                || string.IsNullOrWhiteSpace(Formulario.clave))
            {
                ViewData["Mensaje"] = "Usuario, correo y clave son obligatorios.";
                await CargarListasAsync();
                return Page();
            }

            if (Formulario.rol <= 0 || Formulario.persona <= 0)
            {
                ViewData["Mensaje"] = "Seleccione un rol y una persona válidos.";
                await CargarListasAsync();
                return Page();
            }

            if (Formulario.clave != ConfirmarClave)
            {
                ViewData["Mensaje"] = "La clave y la confirmación no coinciden.";
                await CargarListasAsync();
                return Page();
            }

            try
            {
                ViewData["Mensaje"] = await autenticacion.CrearUsuario(Formulario);
                Formulario = new PedidoCrearUsuario();
                ConfirmarClave = "";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarListasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCerrarAsync()
        {
            ViewData.Remove("Mensaje");
            await CargarListasAsync();
            return Page();
        }

        private async Task CargarListasAsync()
        {
            try
            {
                ListaRoles = await roles.Consultar();
                ListaPersonas = await personas.Consultar();
                OpcionesPersonas = ListaPersonas
                    .Select(x => new SelectListItem(
                        $"{x.nombre} ({x.cedula})", x.id.ToString()))
                    .ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }
    }
}
