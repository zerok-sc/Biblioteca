using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class Libros_CategoriasController : ControladorBase<Libros_Categorias>
    {
        public Libros_CategoriasController(IRepository<Libros_Categorias> repositorio) : base(repositorio)
        {
        }
    }
}
