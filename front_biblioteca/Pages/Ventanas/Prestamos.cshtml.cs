using System.Globalization;
using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = "ADMIN,BIBLIO")]
    public class PrestamosModel : PaginaBase<Prestamos>
    {
        public List<Personas> Coincidencias { get; set; } = new();

        [BindProperty] public string? BuscarNombre { get; set; }
        [BindProperty] public int PersonaSeleccionada { get; set; }
        [BindProperty] public int EmpleadoSeleccionado { get; set; }
        [BindProperty] public int Tiempo { get; set; } = 15;
        [BindProperty] public DateTime FechaInicio { get; set; } = DateTime.Today;
        [BindProperty] public DateTime? FechaFin { get; set; } = DateTime.Today.AddDays(15);
        [BindProperty] public int EstadoSeleccionado { get; set; }

        public string NombrePersonaSeleccionada =>
            PersonaSeleccionada == 0 ? "Ninguno" : NombrePersona(PersonaSeleccionada);

        public List<Estados> EstadosPrestamo
        {
            get
            {
                var lista = Catalogos.ListaEstados
                    .Where(x => x.codigo == "PRE" || x.codigo == "DEV" || x.codigo == "ATR")
                    .ToList();

                return lista.Count > 0 ? lista : Catalogos.ListaEstados;
            }
        }

        public PrestamosModel(
            IPresentacion<Prestamos> presentacion,
            Catalogos catalogos) : base(presentacion, catalogos)
        {
        }

        protected override Task CargarCatalogosAsync()
        {
            if (EstadoSeleccionado == 0)
            {
                EstadoSeleccionado = Catalogos.ListaEstados
                    .FirstOrDefault(x => x.codigo == "PRE")?.id
                    ?? Catalogos.ListaEstados.FirstOrDefault()?.id
                    ?? 0;
            }

            if (EmpleadoSeleccionado == 0)
                EmpleadoSeleccionado = Catalogos.ListaEmpleados.FirstOrDefault()?.id ?? 0;

            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostBuscarAsync()
        {
            await base.OnGetAsync();

            var texto = BuscarNombre?.Trim() ?? "";
            Coincidencias = string.IsNullOrWhiteSpace(texto)
                ? new List<Personas>()
                : Catalogos.ListaPersonas
                    .Where(x => Contiene(x.nombre, texto) || Contiene(x.cedula, texto))
                    .Take(20)
                    .ToList();

            PersonaSeleccionada = 0;
            MostrarFormulario = false;
            return Page();
        }

        public async Task<IActionResult> OnPostSeleccionarAsync(int personaId)
        {
            await base.OnGetAsync();

            var persona = Catalogos.ListaPersonas.FirstOrDefault(x => x.id == personaId);
            if (persona is null)
            {
                Avisar("Seleccione un usuario válido.");
                return Page();
            }

            PersonaSeleccionada = persona.id;
            BuscarNombre = persona.nombre;
            Coincidencias.Clear();
            MostrarFormulario = true;
            return Page();
        }

        public async Task<IActionResult> OnPostCrearPrestamoAsync()
        {
            await base.OnGetAsync();
            MostrarFormulario = true;

            try
            {
                var persona = Catalogos.ListaPersonas.FirstOrDefault(x => x.id == PersonaSeleccionada);
                if (persona is null)
                    throw new InvalidOperationException("Primero busque y seleccione el usuario del préstamo.");

                var empleado = Catalogos.ListaEmpleados.FirstOrDefault(x => x.id == EmpleadoSeleccionado);
                if (empleado is null)
                    throw new InvalidOperationException("Seleccione el empleado que registra el préstamo.");

                var estado = Catalogos.ListaEstados.FirstOrDefault(x => x.id == EstadoSeleccionado);
                if (estado is null)
                    throw new InvalidOperationException("Seleccione un estado válido.");

                var inicio = FechaInicio.Date;
                if (inicio == default)
                    throw new InvalidOperationException("Indique la fecha de inicio.");

                if (Tiempo <= 0)
                    throw new InvalidOperationException("El tiempo debe ser mayor que cero días.");

                var fin = FechaFin?.Date ?? inicio.AddDays(Tiempo);
                if (fin < inicio)
                    throw new InvalidOperationException("La fecha final no puede ser anterior a la fecha de inicio.");

                var prestamo = new Prestamos
                {
                    codigo = $"PRES-{DateTime.Now:yyyyMMddHHmmssfff}",
                    persona = persona.id,
                    empleado = empleado.id,
                    fecha = inicio,
                    tiempo = Tiempo,
                    fecha_inicio = inicio,
                    fecha_fin = fin,
                    estado = estado.id
                };

                await presentacion.Insertar(prestamo);
                Avisar($"Préstamo registrado para {persona.nombre}.");

                MostrarFormulario = false;
                PersonaSeleccionada = 0;
                BuscarNombre = null;
                Coincidencias.Clear();
                Tiempo = 15;
                FechaInicio = DateTime.Today;
                FechaFin = DateTime.Today.AddDays(15);
                await base.OnGetAsync();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
                await base.OnGetAsync();
            }

            return Page();
        }

        private static bool Contiene(string? valor, string texto)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;

            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                valor, texto, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
        }

        public string NombrePersona(int id) => Catalogos.NombrePersona(id);

        public string NombreEmpleado(int id) => Catalogos.NombreEmpleado(id);

        public string NombreEstado(int id) => Catalogos.NombreEstado(id);
    }
}
