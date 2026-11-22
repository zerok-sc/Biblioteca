using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class UbicacionesModel : PaginaBase<Ubicaciones>
    {
        public UbicacionesModel(IPresentacion<Ubicaciones> presentacion) : base(presentacion)
        {
        }
    }
}
