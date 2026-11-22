using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class EmpleadosModel : PaginaBase<Empleados>
    {
        public EmpleadosModel(IPresentacion<Empleados> presentacion) : base(presentacion)
        {
        }
    }
}
