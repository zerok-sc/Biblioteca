using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class Compras_LibrosController : ControladorBase<Compras_Libros>
    {
        public Compras_LibrosController(IRepository<Compras_Libros> repositorio) : base(repositorio)
        {
        }
    }
}
