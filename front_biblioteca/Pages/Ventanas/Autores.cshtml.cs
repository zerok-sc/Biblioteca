using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class AutoresModel : PaginaBase<Autores>
    {
        public AutoresModel(IPresentacion<Autores> presentacion) : base(presentacion)
        {
        }
    }
}
