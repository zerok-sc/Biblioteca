using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class LibrosModel : PaginaBase<Libros>
    {
        public LibrosModel(IPresentacion<Libros> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }

        public int Disponibles(int libroId) =>
            Catalogos.ListaEjemplares
                .Count(x => x.libro == libroId && Catalogos.CodigoEstado(x.estado) == "DIS");
    }
}
