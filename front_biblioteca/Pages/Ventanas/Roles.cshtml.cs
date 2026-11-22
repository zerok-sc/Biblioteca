using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class RolesModel : PaginaBase<Roles>
    {
        public RolesModel(IPresentacion<Roles> presentacion) : base(presentacion)
        {
        }
    }
}
