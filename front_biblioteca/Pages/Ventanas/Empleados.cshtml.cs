using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = Codigos.RolesOperador)]
    public class EmpleadosModel : PaginaBase<Empleados>
    {
        public EmpleadosModel(IPresentacion<Empleados> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
