using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize(Roles = "ADMIN,BIBLIO")]
    public class Libros_AutoresModel : PaginaBase<Libros_Autores>
    {
        public Libros_AutoresModel(IPresentacion<Libros_Autores> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
