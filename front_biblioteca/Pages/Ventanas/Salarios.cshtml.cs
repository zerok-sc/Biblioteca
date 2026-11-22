using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = "ADMIN,BIBLIO")]
    public class SalariosModel : PaginaBase<Salarios>
    {
        public SalariosModel(IPresentacion<Salarios> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
