using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class SalariosModel : PaginaBase<Salarios>
    {
        public SalariosModel(IPresentacion<Salarios> presentacion) : base(presentacion)
        {
        }
    }
}
