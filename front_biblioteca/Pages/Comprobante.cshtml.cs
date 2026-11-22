using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    [Authorize(Roles = "ADMIN,BIBLIO")]
    public class ComprobanteModel : PageModel
    {
        private readonly Catalogos catalogos;

        public Catalogos Catalogos => catalogos;

        public string Tipo { get; private set; } = "";
        public Prestamos? Prestamo { get; private set; }
        public Multas? Multa { get; private set; }
        public List<Prestamos_Libros> Detalles { get; private set; } = new();
        public List<Multas> MultasPrestamo { get; private set; } = new();

        public ComprobanteModel(Catalogos catalogos)
        {
            this.catalogos = catalogos;
        }

        public async Task<IActionResult> OnGetAsync(string tipo, int id)
        {
            await catalogos.CargarAsync();
            Tipo = tipo;

            if (tipo == "prestamo")
            {
                Prestamo = catalogos.ListaPrestamos.FirstOrDefault(x => x.id == id);
                if (Prestamo is null)
                    return NotFound();

                Detalles = catalogos.PrestamosLibrosDe(id);
                var ids = Detalles.Select(x => x.id).ToHashSet();
                MultasPrestamo = catalogos.ListaMultas
                    .Where(x => ids.Contains(x.prestamo_libro))
                    .ToList();

                return Page();
            }

            if (tipo == "multa")
            {
                Multa = catalogos.ListaMultas.FirstOrDefault(x => x.id == id);
                if (Multa is null)
                    return NotFound();

                return Page();
            }

            return NotFound();
        }

        public string NombrePersona(int id) => catalogos.NombrePersona(id);

        public string NombreEmpleado(int id) => catalogos.NombreEmpleado(id);

        public string NombreEstado(int id) => catalogos.NombreEstado(id);

        public string NombreLibroDeEjemplar(int id) => catalogos.NombreLibroDeEjemplar(id);

        public string NombreUbicacion(int id) => catalogos.NombreUbicacion(id);

        public string NombreUbicacionDeEjemplar(int ejemplarId)
        {
            var ejemplar = catalogos.ListaEjemplares.FirstOrDefault(x => x.id == ejemplarId);
            return ejemplar is null ? "No asignado" : catalogos.NombreUbicacion(ejemplar.ubicacion);
        }

        public string NombrePersonaMulta(Multas multa) =>
            catalogos.NombrePersonaDePrestamoLibro(multa.prestamo_libro);

        public string NombreLibroMulta(Multas multa) =>
            catalogos.NombreLibroDePrestamoLibro(multa.prestamo_libro);

        public int DiasAtraso(Multas multa) =>
            catalogos.DiasAtrasoDePrestamoLibro(multa.prestamo_libro);

        public string Situacion(Prestamos prestamo)
        {
            if (!catalogos.PrestamoActivo(prestamo))
                return "Devuelto";

            if (catalogos.PrestamoAtrasado(prestamo))
                return $"{catalogos.DiasAtraso(prestamo)} día(s) de atraso";

            var dias = catalogos.DiasRestantes(prestamo);
            return dias == 0 ? "Vence hoy" : $"Vence en {dias} día(s)";
        }

        public decimal TotalMultas => MultasPrestamo.Sum(x => x.valor);
    }
}
