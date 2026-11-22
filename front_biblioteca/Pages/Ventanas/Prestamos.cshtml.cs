using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class PrestamosModel : PaginaBase<Prestamos>
    {
        public PrestamosModel(IPresentacion<Prestamos> presentacion) : base(presentacion)
        {
        }
    }
}
