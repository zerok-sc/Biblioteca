using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = "ADMIN")]
    public class UsuariosModel : PaginaBase<Usuarios>
    {
        private readonly IPresentacion<Roles> roles;
        private readonly IPresentacion<Personas> personas;

        public List<Roles> ListaRoles { get; set; } = new();
        public List<Personas> ListaPersonas { get; set; } = new();

        public UsuariosModel(
            IPresentacion<Usuarios> presentacion,
            IPresentacion<Roles> roles,
            IPresentacion<Personas> personas) : base(presentacion)
        {
            this.roles = roles;
            this.personas = personas;
        }

        protected override async Task CargarCatalogosAsync()
        {
            try
            {
                ListaRoles = await roles.Consultar();
                ListaPersonas = await personas.Consultar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }
    }
}
