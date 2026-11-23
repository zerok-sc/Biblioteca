using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = Codigos.RolesOperador)]
    public class Prestamos_LibrosModel : PaginaBase<Prestamos_Libros>
    {
        public Prestamos_LibrosModel(IPresentacion<Prestamos_Libros> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
