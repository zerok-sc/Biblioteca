using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = Codigos.RolesOperador)]
    public class Libros_CategoriasModel : PaginaBase<Libros_Categorias>
    {
        public Libros_CategoriasModel(IPresentacion<Libros_Categorias> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
