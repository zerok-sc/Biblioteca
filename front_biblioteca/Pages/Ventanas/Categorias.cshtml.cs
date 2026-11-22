using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class CategoriasModel : PaginaBase<Categorias>
    {
        public CategoriasModel(IPresentacion<Categorias> presentacion) : base(presentacion)
        {
        }
    }
}
