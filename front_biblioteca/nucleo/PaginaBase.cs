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
        public List<T> Lista { get; set; } = new();
        public bool MostrarFormulario { get; set; }

        protected PaginaBase(IPresentacion<T> presentacion)
        {
            this.presentacion = presentacion;
        }

        private bool SoloLectura => User.IsInRole("LECTOR");

        public async Task OnGetAsync()
        {
            await CargarAsync();
        }

        public async Task OnPostConsultarAsync()
        {
            ViewData.Remove("Mensaje");
            try
            {
                Lista = FiltroId > 0
                    ? await presentacion.Buscar(new T { id = FiltroId })
                    : await presentacion.Consultar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }

        public async Task OnPostNuevoAsync()
        {
            if (SoloLectura)
            {
                Avisar("El rol Lector solo puede consultar.");
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
                return;
            }

            try
            {
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
                await CargarCatalogosAsync();
            }
            catch (Exception ex)
            {
                LogConversor.Log(new Exception("No se pudo consultar la WebAPI: " + ex.Message), ViewData);
                Lista = new List<T>();
            }
        }
    }
}
