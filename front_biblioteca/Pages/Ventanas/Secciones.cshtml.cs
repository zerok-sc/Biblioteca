using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class SeccionesModel : PaginaBase<Secciones>
    {
        public SeccionesModel(IPresentacion<Secciones> presentacion) : base(presentacion)
        {
        }
    }
}
