using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = "ADMIN,BIBLIO")]
    public class MultasModel : PaginaBase<Multas>
    {
        public MultasModel(IPresentacion<Multas> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
