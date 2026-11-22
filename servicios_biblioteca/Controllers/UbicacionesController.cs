using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class UbicacionesController : ControladorBase<Ubicaciones>
    {
        public UbicacionesController(IRepository<Ubicaciones> repositorio) : base(repositorio)
        {
        }
    }
}
