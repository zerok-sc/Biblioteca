using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class UsuariosController : ControladorBase<Usuarios>
    {
        public UsuariosController(IRepository<Usuarios> repositorio) : base(repositorio)
        {
        }
    }
}
