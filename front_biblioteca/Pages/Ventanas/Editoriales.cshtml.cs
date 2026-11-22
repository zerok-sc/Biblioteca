using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class EditorialesModel : PaginaBase<Editoriales>
    {
        public EditorialesModel(IPresentacion<Editoriales> presentacion) : base(presentacion)
        {
        }
    }
}
