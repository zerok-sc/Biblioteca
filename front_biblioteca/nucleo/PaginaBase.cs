using System.Text.Json;
using aplicaciones_libreria.entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using presentacion_libreria.interfaces;

namespace front_biblioteca.nucleo
{
    public abstract class PaginaBase<T> : PageModel where T : class, IEntidad, new()
    {
        protected readonly IPresentacion<T> presentacion;

        [BindProperty] public T Actual { get; set; } = new();
        [BindProperty] public int FiltroId { get; set; }
        [BindProperty] public string? Buscar { get; set; }
        public List<T> Lista { get; set; } = new();
        public bool MostrarFormulario { get; set; }
        public Catalogos Catalogos { get; }

        protected PaginaBase(IPresentacion<T> presentacion, Catalogos catalogos)
        {
            this.presentacion = presentacion;
            Catalogos = catalogos;
        }

        private bool SoloLectura => User.IsInRole(Codigos.RolLector);

        private static bool EsSimple(Type tipo)
        {
            var baseTipo = Nullable.GetUnderlyingType(tipo) ?? tipo;
            return baseTipo.IsPrimitive || baseTipo.IsEnum || baseTipo == typeof(string)
                || baseTipo == typeof(decimal) || baseTipo == typeof(DateTime)
                || baseTipo == typeof(Guid);
        }

        protected static Dictionary<string, object?>? Snapshot(object? entidad)
        {
            if (entidad is null)
                return null;

            return entidad.GetType().GetProperties()
                .Where(p => EsSimple(p.PropertyType))
                .ToDictionary(p => p.Name, p => p.GetValue(entidad));
        }

        protected async Task RegistrarAuditoriaAsync(
            string operacion,
            Dictionary<string, object?>? anteriores,
            object? nuevos,
            int entidadId,
            string? entidad = null)
        {
            if (typeof(T) == typeof(Auditorias))
                return;

            try
            {
                var servicio = HttpContext.RequestServices.GetRequiredService<IAuditoriaPresentacion>();
                await servicio.Registrar(new Auditorias
                {
                    entidad = entidad ?? typeof(T).Name,
                    entidad_id = entidadId,
                    operacion = operacion,
                    fecha = DateTime.Now,
                    valores_anteriores = anteriores is null ? null : JsonSerializer.Serialize(anteriores),
                    valores_nuevos = nuevos is null ? null : JsonSerializer.Serialize(Snapshot(nuevos)),
                    nombre_usuario = User.Identity?.Name ?? "",
                    direccion_ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? ""
                });
            }
            catch
            {
                // La auditoría no debe impedir la operación principal.
            }
        }

        private static bool Coincide(T entidad, string texto)
        {
            foreach (var propiedad in typeof(T).GetProperties())
            {
                if (propiedad.PropertyType != typeof(string))
                    continue;

                var valor = propiedad.GetValue(entidad) as string;
                if (!string.IsNullOrWhiteSpace(valor)
                    && valor.Contains(texto, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public virtual async Task OnGetAsync()
        {
            await CargarAsync();
        }

        public virtual async Task OnPostConsultarAsync()
        {
            ViewData.Remove("Mensaje");
            try
            {
                var todos = await presentacion.Consultar();
                var texto = Buscar?.Trim();

                Lista = FiltroId > 0
                    ? todos.Where(x => x.id == FiltroId).ToList()
                    : string.IsNullOrWhiteSpace(texto)
                        ? todos
                        : todos.Where(x => Coincide(x, texto!)).ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarCatalogosSeguroAsync();
        }

        public virtual async Task OnPostNuevoAsync()
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
                await CargarAsync();
                return;
            }

            Actual = new T();
            MostrarFormulario = true;
            await CargarAsync();
        }

        public virtual async Task OnPostEditarAsync(int id)
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
                await CargarAsync();
                return;
            }

            await CargarAsync();
            Actual = Lista.FirstOrDefault(x => x.id == id) ?? new T { id = id };
            MostrarFormulario = true;
        }

        public virtual async Task OnPostGuardarAsync()
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
                await CargarAsync();
                return;
            }

            try
            {
                var error = await ValidarAsync();
                if (error is not null)
                {
                    Avisar(error);
                    await CargarAsync();
                    return;
                }

                var esAlta = Actual.id == 0;
                var anteriores = esAlta ? null : Snapshot(
                    (await presentacion.Consultar()).FirstOrDefault(x => x.id == Actual.id));

                Actual = esAlta
                    ? await presentacion.Insertar(Actual)
                    : await presentacion.Actualizar(Actual);

                await RegistrarAuditoriaAsync(
                    esAlta ? "ALTA" : "EDICION", anteriores, Actual, Actual.id);

                Avisar("Registro guardado correctamente.");
                MostrarFormulario = false;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarAsync();
        }

        public virtual async Task OnPostBorrarAsync(int id)
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
                await CargarAsync();
                return;
            }

            try
            {
                var anteriores = Snapshot((await presentacion.Consultar()).FirstOrDefault(x => x.id == id));
                await presentacion.Borrar(new T { id = id });
                await RegistrarAuditoriaAsync("BAJA", anteriores, null, id);
                Avisar("Registro eliminado correctamente.");
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarAsync();
        }

        public async Task OnPostCerrarAsync()
        {
            ViewData.Remove("Mensaje");
            await CargarAsync();
        }

        private async Task<string?> ValidarAsync()
        {
            var propiedades = typeof(T).GetProperties();

            var propiedadNombre = propiedades.FirstOrDefault(
                x => x.Name == "nombre" && x.PropertyType == typeof(string));
            if (propiedadNombre is not null)
            {
                var nombre = (propiedadNombre.GetValue(Actual) as string)?.Trim();
                if (string.IsNullOrWhiteSpace(nombre))
                    return "El nombre es obligatorio.";
                propiedadNombre.SetValue(Actual, nombre);
            }

            var propiedadCodigo = propiedades.FirstOrDefault(
                x => x.Name == "codigo" && x.PropertyType == typeof(string));
            if (propiedadCodigo is not null)
            {
                var codigo = (propiedadCodigo.GetValue(Actual) as string)?.Trim();
                if (string.IsNullOrWhiteSpace(codigo))
                    return "El código es obligatorio.";

                var todos = await presentacion.Consultar();
                var repetido = todos.Any(x => x.id != Actual.id
                    && string.Equals(
                        propiedadCodigo.GetValue(x) as string,
                        codigo,
                        StringComparison.OrdinalIgnoreCase));

                if (repetido)
                    return "Ese código ya está registrado.";

                propiedadCodigo.SetValue(Actual, codigo);
            }

            return null;
        }

        protected void Avisar(string mensaje)
        {
            ViewData["Mensaje"] = mensaje;
        }

        protected virtual Task CargarCatalogosAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task CargarAsync()
        {
            try
            {
                Lista = await presentacion.Consultar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(new Exception("No se pudo consultar la WebAPI: " + ex.Message), ViewData);
                Lista = new List<T>();
            }

            await CargarCatalogosSeguroAsync();
            await CargarCatalogosAsync();
        }

        private async Task CargarCatalogosSeguroAsync()
        {
            try
            {
                await Catalogos.CargarAsync();
            }
            catch (Exception ex)
            {
                LogConversor.Log(new Exception("No se pudieron cargar los catálogos: " + ex.Message), ViewData);
            }
        }
    }
}
