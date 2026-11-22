using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class Libros_AutoresController : ControladorBase<Libros_Autores>
    {
        public Libros_AutoresController(IRepository<Libros_Autores> repositorio) : base(repositorio)
        {
        }
    }
}
