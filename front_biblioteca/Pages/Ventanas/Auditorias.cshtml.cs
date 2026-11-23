using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = Codigos.RolAdmin)]
    public class AuditoriasModel : PaginaBase<Auditorias>
    {
        public AuditoriasModel(
            IPresentacion<Auditorias> presentacion,
            Catalogos catalogos) : base(presentacion, catalogos)
        {
        }

        public override async Task OnPostNuevoAsync()
        {
            Avisar("Auditorías es solo lectura.");
            await CargarAsync();
        }

        public override async Task OnPostEditarAsync(int id)
        {
            Avisar("Auditorías es solo lectura.");
            await CargarAsync();
        }

        public override async Task OnPostGuardarAsync()
        {
            Avisar("Auditorías es solo lectura.");
            await CargarAsync();
        }

        public override async Task OnPostBorrarAsync(int id)
        {
            Avisar("Auditorías es solo lectura.");
            await CargarAsync();
        }
    }
}
