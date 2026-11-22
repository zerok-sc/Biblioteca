using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class EstanteriasModel : PaginaBase<Estanterias>
    {
        public EstanteriasModel(IPresentacion<Estanterias> presentacion) : base(presentacion)
        {
        }
    }
}
