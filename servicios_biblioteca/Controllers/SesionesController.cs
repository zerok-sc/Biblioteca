using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class SesionesController : ControladorBase<Sesiones>
    {
        public SesionesController(IRepository<Sesiones> repositorio) : base(repositorio)
        {
        }
    }
}
