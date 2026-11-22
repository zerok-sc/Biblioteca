using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentacion_libreria.interfaces;

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

        private readonly IPresentacion<Libros> libros;
        private readonly IPresentacion<Autores> autores;
        private readonly IPresentacion<Prestamos> prestamos;
        private readonly IPresentacion<Usuarios> usuarios;

        public int TotalLibros { get; private set; }
        public int TotalAutores { get; private set; }
        public int TotalPrestamos { get; private set; }
        public int TotalUsuarios { get; private set; }

        public IndexModel(
            IPresentacion<Libros> libros,
            IPresentacion<Autores> autores,
            IPresentacion<Prestamos> prestamos,
            IPresentacion<Usuarios> usuarios)
        {
            this.libros = libros;
            this.autores = autores;
            this.prestamos = prestamos;
            this.usuarios = usuarios;
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
                TotalLibros = (await libros.Consultar()).Count;
                TotalAutores = (await autores.Consultar()).Count;
                TotalPrestamos = (await prestamos.Consultar()).Count;
                TotalUsuarios = (await usuarios.Consultar()).Count;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }
        }
    }
}
