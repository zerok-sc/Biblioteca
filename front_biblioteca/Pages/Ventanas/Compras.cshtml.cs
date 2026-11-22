using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class ComprasModel : PaginaBase<Compras>
    {
        public ComprasModel(IPresentacion<Compras> presentacion) : base(presentacion)
        {
        }
    }
}
