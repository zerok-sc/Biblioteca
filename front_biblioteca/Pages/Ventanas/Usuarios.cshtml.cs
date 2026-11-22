using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class UsuariosModel : PaginaBase<Usuarios>
    {
        public UsuariosModel(IPresentacion<Usuarios> presentacion) : base(presentacion)
        {
        }
    }
}
