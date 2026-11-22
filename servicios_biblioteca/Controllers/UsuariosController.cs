using aplicaciones_libreria.entidades;
using aplicaciones_libreria.implementaciones;
using aplicaciones_libreria.interfaces;
using aplicaciones_libreria.nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace servicios_biblioteca.Controllers
{
    public class UsuariosController : ControladorBase<Usuarios>
    {
        private readonly Conexion conexion;

        public UsuariosController(IRepository<Usuarios> repositorio, Conexion conexion)
            : base(repositorio)
        {
            this.conexion = conexion;
        }

        [HttpPost]
        public async Task<ActionResult<DatosSesion>> Login([FromBody] PedidoLogin pedido)
        {
            var usuario = await conexion.Usuarios!
                .Include(x => x._rol)
                .Include(x => x._persona)
                .FirstOrDefaultAsync(x => x.nombre_usuario == pedido.usuario);

            if (usuario is null || !usuario.activo)
                return Unauthorized("Usuario o clave incorrectos.");

            var calculado = ClaveSeguridad.Hash(pedido.clave, usuario.password_salt);
            if (!ClaveSeguridad.Coinciden(usuario.password_hash, calculado))
                return Unauthorized("Usuario o clave incorrectos.");

            await conexion.Usuarios!
                .Where(x => x.id == usuario.id)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.ultimo_acceso, DateTime.Now));

            return new DatosSesion
            {
                id = usuario.id,
                nombre_usuario = usuario.nombre_usuario ?? "",
                correo = usuario.correo ?? "",
                rol_codigo = usuario._rol?.codigo ?? "",
                rol_nombre = usuario._rol?.nombre ?? "",
                persona_nombre = usuario._persona?.nombre ?? "",
                imagen = usuario.foto is { Length: > 0 }
                    ? Convert.ToBase64String(usuario.foto)
                    : null,
                imagen_tipo = usuario.foto_tipo
            };
        }

        [HttpPost]
        public async Task<ActionResult<DatosFoto>> ObtenerFoto([FromBody] PedidoFoto pedido)
        {
            var foto = await conexion.Usuarios!
                .Where(x => x.id == pedido.id)
                .Select(x => x.foto)
                .FirstOrDefaultAsync();

            return new DatosFoto
            {
                imagen = foto is { Length: > 0 } ? Convert.ToBase64String(foto) : null
            };
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaSimple>> CambiarFoto([FromBody] PedidoFoto pedido)
        {
            byte[]? imagen = string.IsNullOrEmpty(pedido.imagen)
                ? null
                : Convert.FromBase64String(pedido.imagen);

            await conexion.Usuarios!
                .Where(x => x.id == pedido.id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.foto, imagen)
                    .SetProperty(u => u.foto_tipo, pedido.tipo));

            return new RespuestaSimple { mensaje = "Foto actualizada." };
        }
    }
}
