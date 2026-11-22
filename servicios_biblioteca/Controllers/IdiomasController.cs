using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class IdiomasController : ControladorBase<Idiomas>
    {
        public IdiomasController(IRepository<Idiomas> repositorio) : base(repositorio)
        {
        }
    }
}
