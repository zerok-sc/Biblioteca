using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class EstadosModel : PaginaBase<Estados>
    {
        public EstadosModel(IPresentacion<Estados> presentacion) : base(presentacion)
        {
        }
    }
}
