using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class PrestamosController : ControladorBase<Prestamos>
    {
        public PrestamosController(IRepository<Prestamos> repositorio) : base(repositorio)
        {
        }
    }
}
