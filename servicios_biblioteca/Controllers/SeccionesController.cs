using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class SeccionesController : ControladorBase<Secciones>
    {
        public SeccionesController(IRepository<Secciones> repositorio) : base(repositorio)
        {
        }
    }
}
