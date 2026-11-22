using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class CategoriasController : ControladorBase<Categorias>
    {
        public CategoriasController(IRepository<Categorias> repositorio) : base(repositorio)
        {
        }
    }
}
