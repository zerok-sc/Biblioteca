using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class ComprasController : ControladorBase<Compras>
    {
        public ComprasController(IRepository<Compras> repositorio) : base(repositorio)
        {
        }
    }
}
