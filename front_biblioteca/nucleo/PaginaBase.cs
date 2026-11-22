using aplicaciones_libreria.entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

namespace front_biblioteca.nucleo
{
    public abstract class PaginaBase<T> : PageModel where T : class, IEntidad, new()
    {
        private readonly IPresentacion<T> presentacion;

        [BindProperty] public T Actual { get; set; } = new();
        public List<T> Lista { get; set; } = new();
        public string? Mensaje { get; set; }
        public string? Error { get; set; }
        public bool MostrarFormulario { get; set; }

        protected PaginaBase(IPresentacion<T> presentacion)
        {
            this.presentacion = presentacion;
        }

        public async Task OnGetAsync()
        {
            await CargarAsync();
        }

        public async Task OnPostNuevoAsync()
        {
            Actual = new T();
            MostrarFormulario = true;
            await CargarAsync();
        }

        public async Task OnPostEditarAsync(int id)
        {
            await CargarAsync();
            Actual = Lista.FirstOrDefault(x => x.id == id) ?? new T { id = id };
            MostrarFormulario = true;
        }

        public async Task OnPostGuardarAsync()
        {
            try
            {
                Actual = Actual.id == 0
                    ? await presentacion.Insertar(Actual)
                    : await presentacion.Actualizar(Actual);
                Mensaje = "Registro guardado correctamente.";
                MostrarFormulario = false;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            await CargarAsync();
        }

        public async Task OnPostBorrarAsync(int id)
        {
            try
            {
                await presentacion.Borrar(new T { id = id });
                Mensaje = "Registro eliminado correctamente.";
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            await CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                Lista = await presentacion.Consultar();
            }
            catch (Exception ex)
            {
                Error = $"No se pudo consultar la WebAPI: {ex.Message}";
                Lista = new List<T>();
            }
        }
    }
}
