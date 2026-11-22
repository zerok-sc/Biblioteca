using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class SeccionesModel : PaginaBase<Secciones>
    {
        public SeccionesModel(IPresentacion<Secciones> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
