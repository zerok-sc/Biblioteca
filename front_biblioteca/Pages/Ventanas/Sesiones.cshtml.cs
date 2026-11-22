using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = "ADMIN")]
    public class SesionesModel : PaginaBase<Sesiones>
    {
        public SesionesModel(IPresentacion<Sesiones> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
