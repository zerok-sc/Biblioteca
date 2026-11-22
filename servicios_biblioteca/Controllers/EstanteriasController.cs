using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class EstanteriasController : ControladorBase<Estanterias>
    {
        public EstanteriasController(IRepository<Estanterias> repositorio) : base(repositorio)
        {
        }
    }
}
