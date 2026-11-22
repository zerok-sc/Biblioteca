using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class EjemplaresModel : PaginaBase<Ejemplares>
    {
        public EjemplaresModel(IPresentacion<Ejemplares> presentacion) : base(presentacion)
        {
        }
    }
}
