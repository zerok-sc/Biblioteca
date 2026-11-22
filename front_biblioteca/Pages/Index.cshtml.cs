using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front_biblioteca.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private static readonly string[] Seguridad =
            { "Usuarios", "Roles", "Auditorias", "Sesiones" };

        private static readonly string[] Catalogo =
            { "Libros", "Autores", "Categorias", "Editoriales", "Idiomas",
              "Ejemplares", "Secciones", "Estanterias", "Ubicaciones" };

        private readonly Catalogos catalogos;

        public bool EsOperador => User.IsInRole("ADMIN") || User.IsInRole("BIBLIO");

        public int TotalLibros { get; private set; }
        public int TotalAutores { get; private set; }
        public int TotalPrestamos { get; private set; }
        public int TotalUsuarios { get; private set; }
        public int PrestamosActivos { get; private set; }
        public int PrestamosAtrasados { get; private set; }
        public int PrestamosPorVencer { get; private set; }
        public int EjemplaresDisponibles { get; private set; }
        public int MultasPendientes { get; private set; }
        public decimal ValorMultasPendientes { get; private set; }

        public List<Prestamos> Atrasados { get; private set; } = new();
        public List<Prestamos> PorVencer { get; private set; } = new();

        public IndexModel(Catalogos catalogos)
        {
            this.catalogos = catalogos;
        }

        public bool PuedeVer(string entidad)
        {
            if (User.IsInRole("ADMIN"))
                return true;

            if (User.IsInRole("BIBLIO"))
                return !Seguridad.Contains(entidad);

            return Catalogo.Contains(entidad);
        }

        public async Task OnGetAsync()
        {
            try
            {
                await catalogos.CargarAsync();

                TotalLibros = catalogos.ListaLibros.Count;
                TotalAutores = catalogos.ListaAutores.Count;
                TotalPrestamos = catalogos.ListaPrestamos.Count;
                TotalUsuarios = catalogos.ListaUsuarios.Count;

                PrestamosActivos = catalogos.ListaPrestamos.Count(catalogos.PrestamoActivo);
                PrestamosAtrasados = catalogos.ListaPrestamos.Count(catalogos.PrestamoAtrasado);
                PrestamosPorVencer = catalogos.ListaPrestamos.Count(EstaPorVencer);

                EjemplaresDisponibles = catalogos.ListaEjemplares
                    .Count(x => catalogos.CodigoEstado(x.estado) == "DIS");

                var multasActivas = catalogos.ListaMultas
                    .Where(x => catalogos.CodigoEstado(x.estado) == "ACT")
                    .ToList();

                MultasPendientes = multasActivas.Count;
                ValorMultasPendientes = multasActivas.Sum(x => x.valor);

                Atrasados = catalogos.ListaPrestamos
                    .Where(catalogos.PrestamoAtrasado)
                    .OrderBy(x => x.fecha_fin)
                    .Take(5)
                    .ToList();

                PorVencer = catalogos.ListaPrestamos
                    .Where(EstaPorVencer)
                    .OrderBy(x => x.fecha_fin)
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }

        private bool EstaPorVencer(Prestamos prestamo)
        {
            if (!catalogos.PrestamoActivo(prestamo))
                return false;

            var dias = catalogos.DiasRestantes(prestamo);
            return dias >= 0 && dias <= 3;
        }

        public string NombrePersona(int id) => catalogos.NombrePersona(id);

        public string NombreEmpleado(int id) => catalogos.NombreEmpleado(id);

        public string NombreEstado(int id) => catalogos.NombreEstado(id);

        public string Ejemplares(Prestamos prestamo) =>
            catalogos.EjemplaresDePrestamo(prestamo.id);

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
}
