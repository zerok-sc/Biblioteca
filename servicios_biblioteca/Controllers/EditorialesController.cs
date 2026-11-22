using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;

namespace servicios_biblioteca.Controllers
{
    public class EditorialesController : ControladorBase<Editoriales>
    {
        public EditorialesController(IRepository<Editoriales> repositorio) : base(repositorio)
        {
        }
    }
}
