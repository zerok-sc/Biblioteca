using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = "ADMIN,BIBLIO")]
    public class MultasModel : PaginaBase<Multas>
    {
        public MultasModel(IPresentacion<Multas> presentacion, Catalogos catalogos)
            : base(presentacion, catalogos)
        {
        }

        public string NombrePersona(Multas multa) =>
            Catalogos.NombrePersonaDePrestamoLibro(multa.prestamo_libro);

        public string NombreLibro(Multas multa) =>
            Catalogos.NombreLibroDePrestamoLibro(multa.prestamo_libro);

        public int DiasAtraso(Multas multa) =>
            Catalogos.DiasAtrasoDePrestamoLibro(multa.prestamo_libro);

        public bool EsPagada(Multas multa) =>
            Catalogos.CodigoEstado(multa.estado) == "PAG";

        public async Task<IActionResult> OnGetExportarAsync()
        {
            await base.OnGetAsync();

            var filas = Lista.Select(x => new object?[]
            {
                x.id,
                NombrePersona(x),
                NombreLibro(x),
                x.valor.ToString("N0"),
                DiasAtraso(x),
                x.fecha.ToString("dd/MM/yyyy"),
                Catalogos.NombrePrestamoLibro(x.prestamo_libro),
                Catalogos.NombreEstado(x.estado)
            });

            return Exportaciones.Csv(
                $"multas_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                new[] { "id", "Usuario", "Libro", "Valor", "Dias atraso", "Fecha", "Prestamo libro", "Estado" },
                filas);
        }

        public async Task<IActionResult> OnPostPagarAsync(int id)
        {
            await base.OnGetAsync();

            try
            {
                var multa = Catalogos.ListaMultas.FirstOrDefault(x => x.id == id)
                    ?? throw new InvalidOperationException("La multa no existe.");

                if (Catalogos.CodigoEstado(multa.estado) == "PAG")
                    throw new InvalidOperationException("Esa multa ya está pagada.");

                var estadoPagada = Catalogos.IdEstado("PAG");
                if (estadoPagada == 0)
                    throw new InvalidOperationException("No existe el estado Pagada en la base de datos.");

                var anteriorMulta = Snapshot(multa);

                multa.estado = estadoPagada;
                await presentacion.Actualizar(multa);
                await RegistrarAuditoriaAsync("PAGAR", anteriorMulta, multa, multa.id);

                Avisar("Multa marcada como pagada.");
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            try
            {
                await Catalogos.RecargarAsync();
            }
            catch (Exception ex)
            {
                LogConversor.Log(new Exception("No se pudieron recargar los catálogos: " + ex.Message), ViewData);
            }

            await base.OnGetAsync();
            return Page();
        }
    }
}
