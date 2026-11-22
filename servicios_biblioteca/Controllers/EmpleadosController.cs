using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class EmpleadosController : ControladorBase<Empleados>
    {
        public EmpleadosController(IRepository<Empleados> repositorio) : base(repositorio)
        {
        }
    }
}
