using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class ProveedoresController : ControladorBase<Proveedores>
    {
        public ProveedoresController(IRepository<Proveedores> repositorio) : base(repositorio)
        {
        }
    }
}
