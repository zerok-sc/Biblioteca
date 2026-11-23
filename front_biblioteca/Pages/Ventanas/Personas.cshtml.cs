using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = Codigos.RolesOperador)]
    public class PersonasModel : PaginaBase<Personas>
    {
        public PersonasModel(IPresentacion<Personas> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
