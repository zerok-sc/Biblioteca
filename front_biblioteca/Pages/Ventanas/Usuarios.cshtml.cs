using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = Codigos.RolAdmin)]
    public class UsuariosModel : PaginaBase<Usuarios>
    {
        public UsuariosModel(
            IPresentacion<Usuarios> presentacion,
            Catalogos catalogos) : base(presentacion, catalogos)
        {
        }
    }
}
