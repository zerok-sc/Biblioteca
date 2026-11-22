using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class ProveedoresModel : PaginaBase<Proveedores>
    {
        public ProveedoresModel(IPresentacion<Proveedores> presentacion) : base(presentacion)
        {
        }
    }
}
