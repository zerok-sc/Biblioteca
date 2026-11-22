using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class RolesController : ControladorBase<Roles>
    {
        public RolesController(IRepository<Roles> repositorio) : base(repositorio)
        {
        }
    }
}
