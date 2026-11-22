using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class SalariosController : ControladorBase<Salarios>
    {
        public SalariosController(IRepository<Salarios> repositorio) : base(repositorio)
        {
        }
    }
}
