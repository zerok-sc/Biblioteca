using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class MultasController : ControladorBase<Multas>
    {
        public MultasController(IRepository<Multas> repositorio) : base(repositorio)
        {
        }
    }
}
