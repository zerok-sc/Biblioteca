using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class Prestamos_LibrosController : ControladorBase<Prestamos_Libros>
    {
        public Prestamos_LibrosController(IRepository<Prestamos_Libros> repositorio) : base(repositorio)
        {
        }
    }
}
