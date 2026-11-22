using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class UbicacionesModel : PaginaBase<Ubicaciones>
    {
        public UbicacionesModel(IPresentacion<Ubicaciones> presentacion) : base(presentacion)
        {
        }
    }
}
