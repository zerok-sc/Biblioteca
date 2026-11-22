using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class LibrosModel : PaginaBase<Libros>
    {
        public LibrosModel(IPresentacion<Libros> presentacion) : base(presentacion)
        {
        }
    }
}
