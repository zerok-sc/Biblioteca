using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class AutoresModel : PaginaBase<Autores>
    {
        public AutoresModel(IPresentacion<Autores> presentacion) : base(presentacion)
        {
        }
    }
}
