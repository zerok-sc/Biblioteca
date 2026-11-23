using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = Codigos.RolAdmin)]
    public class RolesModel : PaginaBase<Roles>
    {
        public RolesModel(IPresentacion<Roles> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
