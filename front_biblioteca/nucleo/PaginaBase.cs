using aplicaciones_libreria.entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        private bool SoloLectura => User.IsInRole("LECTOR");

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

        public async Task OnGetAsync()
        {
            await CargarAsync();
        }

        public async Task OnPostConsultarAsync()
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

        public async Task OnPostNuevoAsync()
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

        public async Task OnPostEditarAsync(int id)
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

        public async Task OnPostGuardarAsync()
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

                Actual = Actual.id == 0
                    ? await presentacion.Insertar(Actual)
                    : await presentacion.Actualizar(Actual);
                Avisar("Registro guardado correctamente.");
                MostrarFormulario = false;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await CargarAsync();
        }

        public async Task OnPostBorrarAsync(int id)
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
                await CargarAsync();
                return;
            }

            try
            {
                await presentacion.Borrar(new T { id = id });
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

        private async Task CargarAsync()
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
