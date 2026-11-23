using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    [Authorize(Roles = "LECTOR")]
    public class MisPrestamosModel : PageModel
    {
        private readonly Catalogos catalogos;

        public int PersonaId { get; private set; }
        public string NombrePersona { get; private set; } = "";
        public int PrestamosActivos { get; private set; }
        public int PrestamosAtrasados { get; private set; }
        public int PrestamosPorVencer { get; private set; }
        public int MultasPendientes { get; private set; }
        public decimal ValorMultasPendientes { get; private set; }

        public List<Prestamos> Prestamos { get; private set; } = new();
        public List<MultaDetalle> Multas { get; private set; } = new();

        public MisPrestamosModel(Catalogos catalogos)
        {
            this.catalogos = catalogos;
        }

        public async Task OnGetAsync()
        {
            if (!int.TryParse(User.FindFirst("PersonaId")?.Value, out var id) || id <= 0)
            {
                ViewData["Mensaje"] = "No se pudo identificar su persona. Ingrese de nuevo.";
                return;
            }

            PersonaId = id;
            await catalogos.CargarAsync();
            NombrePersona = catalogos.NombrePersona(id);

            Prestamos = catalogos.ListaPrestamos
                .Where(x => x.persona == id)
                .OrderByDescending(x => x.id)
                .ToList();

            PrestamosActivos = Prestamos.Count(catalogos.PrestamoActivo);
            PrestamosAtrasados = Prestamos.Count(catalogos.PrestamoAtrasado);
            PrestamosPorVencer = Prestamos.Count(EstaPorVencer);

            var idsDetalles = Prestamos
                .SelectMany(x => catalogos.PrestamosLibrosDe(x.id))
                .Select(x => x.id)
                .ToHashSet();

            var listaMultas = catalogos.ListaMultas
                .Where(x => idsDetalles.Contains(x.prestamo_libro))
                .ToList();

            MultasPendientes = listaMultas.Count(x => catalogos.CodigoEstado(x.estado) == "ACT");
            ValorMultasPendientes = listaMultas
                .Where(x => catalogos.CodigoEstado(x.estado) == "ACT")
                .Sum(x => x.valor);

            Multas = listaMultas
                .Select(x => new MultaDetalle
                {
                    id = x.id,
                    Libro = catalogos.NombreLibroDePrestamoLibro(x.prestamo_libro),
                    Valor = x.valor,
                    Dias = catalogos.DiasAtrasoDePrestamoLibro(x.prestamo_libro),
                    Estado = catalogos.NombreEstado(x.estado)
                })
                .OrderByDescending(x => x.id)
                .ToList();
        }

        private bool EstaPorVencer(Prestamos prestamo)
        {
            if (!catalogos.PrestamoActivo(prestamo))
                return false;

            var dias = catalogos.DiasRestantes(prestamo);
            return dias >= 0 && dias <= 3;
        }

        public string Ejemplares(Prestamos prestamo) =>
            catalogos.EjemplaresDePrestamo(prestamo.id);

        public string Ubicaciones(Prestamos prestamo) =>
            catalogos.UbicacionesDePrestamo(prestamo.id);

        public string Situacion(Prestamos prestamo)
        {
            if (!catalogos.PrestamoActivo(prestamo))
                return "Devuelto";

            if (catalogos.PrestamoAtrasado(prestamo))
                return $"{catalogos.DiasAtraso(prestamo)} día(s) de atraso";

            var dias = catalogos.DiasRestantes(prestamo);
            return dias == 0 ? "Vence hoy" : $"Vence en {dias} día(s)";
        }

        public string ClaseSituacion(Prestamos prestamo)
        {
            if (!catalogos.PrestamoActivo(prestamo))
                return "etiqueta-ok";

            return catalogos.PrestamoAtrasado(prestamo)
                ? "etiqueta-atrasado"
                : "etiqueta-vencer";
        }
    }

    public class MultaDetalle
    {
        public int id { get; set; }
        public string Libro { get; set; } = "";
        public decimal Valor { get; set; }
        public int Dias { get; set; }
        public string Estado { get; set; } = "";
    }
}
