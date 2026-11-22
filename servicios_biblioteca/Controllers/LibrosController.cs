using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class LibrosController : ControladorBase<Libros>
    {
        public LibrosController(IRepository<Libros> repositorio) : base(repositorio)
        {
        }
    }
}
