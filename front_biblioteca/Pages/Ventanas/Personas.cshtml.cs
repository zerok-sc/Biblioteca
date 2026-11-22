using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class PersonasModel : PaginaBase<Personas>
    {
        public PersonasModel(IPresentacion<Personas> presentacion) : base(presentacion)
        {
        }
    }
}
