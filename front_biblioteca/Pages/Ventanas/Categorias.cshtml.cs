using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class CategoriasModel : PaginaBase<Categorias>
    {
        public CategoriasModel(IPresentacion<Categorias> presentacion) : base(presentacion)
        {
        }
    }
}
