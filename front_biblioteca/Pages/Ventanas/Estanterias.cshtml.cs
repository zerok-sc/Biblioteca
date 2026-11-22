using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class EstanteriasModel : PaginaBase<Estanterias>
    {
        public EstanteriasModel(IPresentacion<Estanterias> presentacion) : base(presentacion)
        {
        }
    }
}
