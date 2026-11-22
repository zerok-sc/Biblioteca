using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class PersonasController : ControladorBase<Personas>
    {
        public PersonasController(IRepository<Personas> repositorio) : base(repositorio)
        {
        }
    }
}
