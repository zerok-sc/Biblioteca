using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class EjemplaresController : ControladorBase<Ejemplares>
    {
        public EjemplaresController(IRepository<Ejemplares> repositorio) : base(repositorio)
        {
        }
    }
}
