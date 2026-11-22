using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class AuditoriasController : ControladorBase<Auditorias>
    {
        public AuditoriasController(IRepository<Auditorias> repositorio) : base(repositorio)
        {
        }
    }
}
