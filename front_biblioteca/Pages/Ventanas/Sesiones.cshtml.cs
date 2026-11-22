using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class SesionesModel : PaginaBase<Sesiones>
    {
        public SesionesModel(IPresentacion<Sesiones> presentacion) : base(presentacion)
        {
        }
    }
}
