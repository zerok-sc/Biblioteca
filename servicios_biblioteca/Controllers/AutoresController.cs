using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class AutoresController : ControladorBase<Autores>
    {
        public AutoresController(IRepository<Autores> repositorio) : base(repositorio)
        {
        }
    }
}
