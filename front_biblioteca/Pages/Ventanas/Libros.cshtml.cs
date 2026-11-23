using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
[Authorize]
    public class LibrosModel : PaginaBase<Libros>
    {
        [BindProperty(SupportsGet = true)] public int CategoriaFiltro { get; set; }

        public LibrosModel(IPresentacion<Libros> presentacion, Catalogos catalogos) : base(presentacion, catalogos)
        {
        }

        public override async Task OnGetAsync()
        {
            await base.OnGetAsync();
            AplicarFiltro();
        }

        public override async Task OnPostConsultarAsync()
        {
            await base.OnPostConsultarAsync();
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            if (CategoriaFiltro > 0)
                Lista = Lista
                    .Where(x => Catalogos.LibroTieneCategoria(x.id, CategoriaFiltro))
                    .ToList();
        }

        public int Disponibles(int libroId) =>
            Catalogos.ListaEjemplares
                .Count(x => x.libro == libroId && Catalogos.CodigoEstado(x.estado) == Codigos.EstadoDisponible);
    }
}
