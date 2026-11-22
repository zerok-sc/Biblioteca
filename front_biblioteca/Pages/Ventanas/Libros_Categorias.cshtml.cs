using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class Libros_CategoriasModel : PaginaBase<Libros_Categorias>
    {
        public Libros_CategoriasModel(IPresentacion<Libros_Categorias> presentacion) : base(presentacion)
        {
        }
    }
}
