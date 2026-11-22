using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class MultasModel : PaginaBase<Multas>
    {
        public MultasModel(IPresentacion<Multas> presentacion) : base(presentacion)
        {
        }
    }
}
