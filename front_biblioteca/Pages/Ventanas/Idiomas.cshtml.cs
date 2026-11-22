using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class IdiomasModel : PaginaBase<Idiomas>
    {
        public IdiomasModel(IPresentacion<Idiomas> presentacion) : base(presentacion)
        {
        }
    }
}
