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
        private const decimal ValorMultaPorDia = 2000m;
        private const int DiasRenovacion = 15;

        private readonly IPresentacion<Prestamos_Libros> prestamosLibros;
        private readonly IPresentacion<Ejemplares> ejemplares;
        private readonly IPresentacion<Multas> multas;

        public List<Personas> Coincidencias { get; set; } = new();

        [BindProperty] public string? BuscarNombre { get; set; }
        [BindProperty] public int PersonaSeleccionada { get; set; }
        [BindProperty] public int EmpleadoSeleccionado { get; set; }
        [BindProperty] public int Tiempo { get; set; } = 15;
        [BindProperty] public DateTime FechaInicio { get; set; } = DateTime.Today;
        [BindProperty] public DateTime? FechaFin { get; set; } = DateTime.Today.AddDays(15);
        [BindProperty] public int EstadoSeleccionado { get; set; }
        [BindProperty] public List<int> EjemplaresSeleccionados { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string? BuscarPrestamo { get; set; }
        [BindProperty(SupportsGet = true)] public string? FiltroEstado { get; set; }

        public string FiltroEstadoActual =>
            string.IsNullOrWhiteSpace(FiltroEstado) ? "todos" : FiltroEstado;

        public string NombrePersonaSeleccionada =>
            PersonaSeleccionada == 0 ? "Ninguno" : NombrePersona(PersonaSeleccionada);

        public List<Ejemplares> EjemplaresDisponibles =>
            Catalogos.ListaEjemplares
                .Where(x => Catalogos.CodigoEstado(x.estado) == "DIS")
                .OrderBy(x => Catalogos.NombreLibro(x.libro))
                .ThenBy(x => x.codigo)
                .ToList();

        public List<Prestamos> PrestamosDePersona =>
            PersonaSeleccionada == 0
                ? new List<Prestamos>()
                : Catalogos.ListaPrestamos
                    .Where(x => x.persona == PersonaSeleccionada)
                    .OrderByDescending(x => x.id)
                    .Take(10)
                    .ToList();

        public List<Prestamos> PrestamosFiltrados
        {
            get
            {
                IEnumerable<Prestamos> lista = Catalogos.ListaPrestamos;
                var texto = BuscarPrestamo?.Trim();

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    lista = lista.Where(x =>
                        Contiene(x.codigo, texto) ||
                        Contiene(NombrePersona(x.persona), texto));
                }

                lista = FiltroEstadoActual switch
                {
                    "activos" => lista.Where(Catalogos.PrestamoActivo),
                    "atrasados" => lista.Where(Catalogos.PrestamoAtrasado),
                    "devueltos" => lista.Where(x => !Catalogos.PrestamoActivo(x)),
                    _ => lista
                };

                return lista.OrderByDescending(x => x.id).ToList();
            }
        }

        public int PrestamosActivos =>
            Catalogos.ListaPrestamos.Count(Catalogos.PrestamoActivo);

        public int PrestamosAtrasados =>
            Catalogos.ListaPrestamos.Count(Catalogos.PrestamoAtrasado);

        public int TotalEjemplaresDisponibles => EjemplaresDisponibles.Count;

        public List<Estados> EstadosPrestamo
        {
            get
            {
                var lista = Catalogos.ListaEstados
                    .Where(x => x.codigo == "PRE" || x.codigo == "ATR")
                    .ToList();

                return lista.Count > 0 ? lista : Catalogos.ListaEstados;
            }
        }

        public PrestamosModel(
            IPresentacion<Prestamos> presentacion,
            IPresentacion<Prestamos_Libros> prestamosLibros,
            IPresentacion<Ejemplares> ejemplares,
            IPresentacion<Multas> multas,
            Catalogos catalogos) : base(presentacion, catalogos)
        {
            this.prestamosLibros = prestamosLibros;
            this.ejemplares = ejemplares;
            this.multas = multas;
        }

        protected override Task CargarCatalogosAsync()
        {
            if (EstadoSeleccionado == 0)
            {
                EstadoSeleccionado = Catalogos.IdEstado("PRE");
                if (EstadoSeleccionado == 0)
                    EstadoSeleccionado = Catalogos.ListaEstados.FirstOrDefault()?.id ?? 0;
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
            EjemplaresSeleccionados.Clear();
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
            EjemplaresSeleccionados.Clear();
            MostrarFormulario = true;
            return Page();
        }

        public async Task<IActionResult> OnPostCrearPrestamoAsync()
        {
            await base.OnGetAsync();
            MostrarFormulario = true;

            Prestamos? creado = null;
            var detallesCreados = new List<Prestamos_Libros>();
            var copiasPrestadas = new List<Ejemplares>();

            try
            {
                var persona = Catalogos.ListaPersonas.FirstOrDefault(x => x.id == PersonaSeleccionada)
                    ?? throw new InvalidOperationException("Primero busque y seleccione el usuario del préstamo.");

                var empleado = Catalogos.ListaEmpleados.FirstOrDefault(x => x.id == EmpleadoSeleccionado)
                    ?? throw new InvalidOperationException("Seleccione el empleado que registra el préstamo.");

                var estado = Catalogos.ListaEstados.FirstOrDefault(x => x.id == EstadoSeleccionado)
                    ?? throw new InvalidOperationException("Seleccione un estado válido.");

                if (estado.codigo is not ("PRE" or "ATR"))
                    throw new InvalidOperationException("Para un préstamo nuevo seleccione el estado Prestado o Atrasado.");

                var inicio = FechaInicio.Date;
                if (inicio == default)
                    throw new InvalidOperationException("Indique la fecha de inicio.");

                if (Tiempo <= 0)
                    throw new InvalidOperationException("El tiempo debe ser mayor que cero días.");

                var fin = FechaFin?.Date ?? inicio.AddDays(Tiempo);
                if (fin < inicio)
                    throw new InvalidOperationException("La fecha final no puede ser anterior a la fecha de inicio.");

                var seleccionados = EjemplaresSeleccionados.Distinct().ToList();
                if (seleccionados.Count == 0)
                    throw new InvalidOperationException("Seleccione al menos un ejemplar disponible.");

                var disponibles = Catalogos.ListaEjemplares
                    .Where(x => seleccionados.Contains(x.id) && Catalogos.CodigoEstado(x.estado) == "DIS")
                    .ToList();

                if (disponibles.Count != seleccionados.Count)
                    throw new InvalidOperationException("Uno de los ejemplares ya no está disponible. Actualice la página e intente de nuevo.");

                var ocupados = Catalogos.ListaPrestamosLibros
                    .Where(x => seleccionados.Contains(x.ejemplar) && Catalogos.EstadoEsActivo(x.estado))
                    .Select(x => x.ejemplar)
                    .Distinct()
                    .ToList();

                if (ocupados.Count > 0)
                    throw new InvalidOperationException("Uno de los ejemplares figura en un préstamo activo. Actualice la página e intente de nuevo.");

                var estadoPrestado = Catalogos.IdEstado("PRE");
                if (estadoPrestado == 0)
                    throw new InvalidOperationException("No existe el estado Prestado en la base de datos.");

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

                creado = await presentacion.Insertar(prestamo);

                foreach (var ejemplar in disponibles)
                {
                    var detalle = await prestamosLibros.Insertar(new Prestamos_Libros
                    {
                        prestamo = creado.id,
                        ejemplar = ejemplar.id,
                        estado = estadoPrestado,
                        fecha = fin
                    });

                    detallesCreados.Add(detalle);

                    ejemplar.estado = estadoPrestado;
                    await ejemplares.Actualizar(ejemplar);
                    copiasPrestadas.Add(ejemplar);
                }

                await RegistrarAuditoriaAsync("PRESTAR", null, creado, creado!.id);

                Avisar($"Préstamo registrado para {persona.nombre} con {disponibles.Count} ejemplar(es).");

                MostrarFormulario = false;
                PersonaSeleccionada = 0;
                BuscarNombre = null;
                Coincidencias.Clear();
                EjemplaresSeleccionados.Clear();
                Tiempo = 15;
                FechaInicio = DateTime.Today;
                FechaFin = DateTime.Today.AddDays(15);
            }
            catch (Exception ex)
            {
                await RevertirCreacionAsync(creado, detallesCreados, copiasPrestadas);
                LogConversor.Log(ex, ViewData);
                await RecargarAsync();
                return Page();
            }

            await RecargarAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDevolverAsync(int id)
        {
            await base.OnGetAsync();

            try
            {
                var prestamo = Catalogos.ListaPrestamos.FirstOrDefault(x => x.id == id)
                    ?? throw new InvalidOperationException("El préstamo no existe.");

                var anteriorPrestamo = Snapshot(prestamo);

                if (!Catalogos.PrestamoActivo(prestamo))
                    throw new InvalidOperationException("Ese préstamo ya fue devuelto.");

                var estadoDevuelto = Catalogos.IdEstado("DEV");
                var estadoDisponible = Catalogos.IdEstado("DIS");
                var estadoActivo = Catalogos.IdEstado("ACT");
                var diasAtraso = Catalogos.DiasAtraso(prestamo);
                var multasCreadas = 0;
                var totalMulta = 0m;

                foreach (var detalle in Catalogos.PrestamosLibrosDe(prestamo.id))
                {
                    if (!Catalogos.EstadoEsActivo(detalle.estado))
                        continue;

                    detalle.estado = estadoDevuelto;
                    await prestamosLibros.Actualizar(detalle);

                    var copia = Catalogos.ListaEjemplares.FirstOrDefault(x => x.id == detalle.ejemplar);
                    if (copia is not null)
                    {
                        copia.estado = estadoDisponible;
                        await ejemplares.Actualizar(copia);
                    }

                    if (diasAtraso > 0)
                    {
                        var yaTieneMulta = Catalogos.ListaMultas.Any(x => x.prestamo_libro == detalle.id);
                        if (!yaTieneMulta)
                        {
                            var valor = diasAtraso * ValorMultaPorDia;
                            var multaCreada = await multas.Insertar(new Multas
                            {
                                valor = valor,
                                fecha = DateTime.Today,
                                prestamo_libro = detalle.id,
                                estado = estadoActivo
                            });

                            await RegistrarAuditoriaAsync("MULTA", null, multaCreada, multaCreada.id, "Multas");

                            multasCreadas++;
                            totalMulta += valor;
                        }
                    }
                }

                prestamo.estado = estadoDevuelto;
                await presentacion.Actualizar(prestamo);
                await RegistrarAuditoriaAsync("DEVOLVER", anteriorPrestamo, prestamo, prestamo.id);

                var mensaje = $"Préstamo {prestamo.codigo} devuelto correctamente.";
                if (multasCreadas > 0)
                    mensaje += $" Se generaron {multasCreadas} multa(s) por {totalMulta:N0} pesos.";

                Avisar(mensaje);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostRenovarAsync(int id)
        {
            await base.OnGetAsync();

            try
            {
                var prestamo = Catalogos.ListaPrestamos.FirstOrDefault(x => x.id == id)
                    ?? throw new InvalidOperationException("El préstamo no existe.");

                if (!Catalogos.PrestamoActivo(prestamo))
                    throw new InvalidOperationException("Ese préstamo ya fue devuelto.");

                if (Catalogos.PrestamoAtrasado(prestamo))
                    throw new InvalidOperationException("No se puede renovar un préstamo atrasado. Primero registre la devolución.");

                var anteriorPrestamo = Snapshot(prestamo);

                prestamo.fecha_fin = prestamo.fecha_fin.Date.AddDays(DiasRenovacion);
                prestamo.tiempo += DiasRenovacion;
                await presentacion.Actualizar(prestamo);

                foreach (var detalle in Catalogos.PrestamosLibrosDe(prestamo.id))
                {
                    detalle.fecha = prestamo.fecha_fin;
                    await prestamosLibros.Actualizar(detalle);
                }

                await RegistrarAuditoriaAsync("RENOVAR", anteriorPrestamo, prestamo, prestamo.id);

                Avisar($"Préstamo {prestamo.codigo} renovado hasta el {prestamo.fecha_fin:dd/MM/yyyy}.");
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostBorrarPrestamoAsync(int id)
        {
            await base.OnGetAsync();

            if (User.IsInRole("LECTOR"))
            {
                Avisar("El rol Lector solo puede consultar.");
                await RecargarAsync();
                return Page();
            }

            try
            {
                var prestamo = Catalogos.ListaPrestamos.FirstOrDefault(x => x.id == id)
                    ?? throw new InvalidOperationException("El préstamo no existe.");

                var anteriores = Snapshot(prestamo);
                var detalles = Catalogos.PrestamosLibrosDe(prestamo.id);
                var idsDetalles = detalles.Select(x => x.id).ToHashSet();
                var multas = Catalogos.ListaMultas
                    .Where(x => idsDetalles.Contains(x.prestamo_libro))
                    .ToList();

                foreach (var multa in multas)
                    await this.multas.Borrar(new Multas { id = multa.id });

                var estadoDisponible = Catalogos.IdEstado("DIS");
                foreach (var detalle in detalles)
                {
                    if (estadoDisponible > 0)
                    {
                        var copia = Catalogos.ListaEjemplares
                            .FirstOrDefault(x => x.id == detalle.ejemplar);
                        if (copia is not null)
                        {
                            copia.estado = estadoDisponible;
                            await ejemplares.Actualizar(copia);
                        }
                    }

                    await prestamosLibros.Borrar(new Prestamos_Libros { id = detalle.id });
                }

                await presentacion.Borrar(new Prestamos { id = id });
                await RegistrarAuditoriaAsync("BAJA", anteriores, null, id);

                var mensaje = $"Préstamo {prestamo.codigo} eliminado.";
                if (detalles.Count > 0)
                    mensaje += $" Se liberaron {detalles.Count} ejemplar(es).";
                if (multas.Count > 0)
                    mensaje += $" Se borraron {multas.Count} multa(s).";
                Avisar(mensaje);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostFiltrarPrestamosAsync()
        {
            await base.OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetExportarAsync()
        {
            await base.OnGetAsync();

            var filas = PrestamosFiltrados.Select(x => new object?[]
            {
                x.id,
                x.codigo,
                NombrePersona(x.persona),
                NombreEmpleado(x.empleado),
                Catalogos.EjemplaresDePrestamo(x.id),
                x.fecha_inicio.ToString("dd/MM/yyyy"),
                x.fecha_fin.ToString("dd/MM/yyyy"),
                x.tiempo,
                EstadoVisible(x),
                Situacion(x)
            });

            return Exportaciones.Csv(
                $"prestamos_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                new[] { "id", "Codigo", "Usuario", "Empleado", "Ejemplares", "Inicio", "Final", "Tiempo", "Estado", "Situacion" },
                filas);
        }

        private async Task RevertirCreacionAsync(
            Prestamos? prestamo,
            List<Prestamos_Libros> detalles,
            List<Ejemplares> copias)
        {
            var estadoDisponible = Catalogos.IdEstado("DIS");

            foreach (var detalle in detalles)
            {
                try
                {
                    await prestamosLibros.Borrar(new Prestamos_Libros { id = detalle.id });
                }
                catch
                {
                    // Si la limpieza falla, se muestra el error original al usuario.
                }
            }

            foreach (var copia in copias)
            {
                try
                {
                    copia.estado = estadoDisponible;
                    await ejemplares.Actualizar(copia);
                }
                catch
                {
                    // Si la limpieza falla, se muestra el error original al usuario.
                }
            }

            if (prestamo is not null)
            {
                try
                {
                    await presentacion.Borrar(new Prestamos { id = prestamo.id });
                }
                catch
                {
                    // Si la limpieza falla, se muestra el error original al usuario.
                }
            }
        }

        private async Task RecargarAsync()
        {
            try
            {
                await Catalogos.RecargarAsync();
            }
            catch (Exception ex)
            {
                LogConversor.Log(new Exception("No se pudieron recargar los catálogos: " + ex.Message), ViewData);
            }

            await base.OnGetAsync();
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

        public string EstadoVisible(Prestamos prestamo) =>
            Catalogos.PrestamoAtrasado(prestamo)
                ? "Atrasado"
                : Catalogos.NombreEstado(prestamo.estado);

        public string Situacion(Prestamos prestamo)
        {
            if (!Catalogos.PrestamoActivo(prestamo))
                return "Devuelto";

            if (Catalogos.PrestamoAtrasado(prestamo))
                return $"{Catalogos.DiasAtraso(prestamo)} día(s) de atraso";

            var dias = Catalogos.DiasRestantes(prestamo);
            return dias == 0 ? "Vence hoy" : $"Vence en {dias} día(s)";
        }

        public string ClaseSituacion(Prestamos prestamo)
        {
            if (!Catalogos.PrestamoActivo(prestamo))
                return "etiqueta-ok";

            return Catalogos.PrestamoAtrasado(prestamo)
                ? "etiqueta-atrasado"
                : "etiqueta-vencer";
        }
    }
}
