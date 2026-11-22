using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class EstadosController : ControladorBase<Estados>
    {
        public EstadosController(IRepository<Estados> repositorio) : base(repositorio)
        {
        }
    }
}
