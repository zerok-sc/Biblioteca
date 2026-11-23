using aplicaciones_libreria.entidades;
using aplicaciones_libreria.implementaciones;
using aplicaciones_libreria.interfaces;
using aplicaciones_libreria.nucleo;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
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

            if (usuario is null)
                return Unauthorized("Usuario o clave incorrectos.");

            if (!usuario.activo)
            {
                await RegistrarSesionAsync(usuario.id, pedido, false, "Usuario inactivo");
                return Unauthorized("Usuario o clave incorrectos.");
            }

            var calculado = ClaveSeguridad.Hash(pedido.clave, usuario.password_salt);
            if (!ClaveSeguridad.Coinciden(usuario.password_hash, calculado))
            {
                await RegistrarSesionAsync(usuario.id, pedido, false, "Clave incorrecta");
                return Unauthorized("Usuario o clave incorrectos.");
            }

            await conexion.Usuarios!
                .Where(x => x.id == usuario.id)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.ultimo_acceso, DateTime.Now));

            var sesionId = await RegistrarSesionAsync(usuario.id, pedido, true, null);

            return new DatosSesion
            {
                sesion_id = sesionId,
                id = usuario.id,
                nombre_usuario = usuario.nombre_usuario ?? "",
                correo = usuario.correo ?? "",
                rol_codigo = usuario._rol?.codigo ?? "",
                rol_nombre = usuario._rol?.nombre ?? "",
                persona_nombre = usuario._persona?.nombre ?? "",
                persona_id = usuario.persona,
                imagen = usuario.foto is { Length: > 0 }
                    ? Convert.ToBase64String(usuario.foto)
                    : null,
                imagen_tipo = usuario.foto_tipo
            };
        }

        private async Task<int> RegistrarSesionAsync(
            int usuarioId,
            PedidoLogin pedido,
            bool exitoso,
            string? motivo)
        {
            var sesion = new Sesiones
            {
                usuario = usuarioId,
                fecha_inicio = DateTime.Now,
                direccion_ip = Limitar(pedido.direccion_ip, 50),
                agente_usuario = Limitar(pedido.agente_usuario, 400),
                exitoso = exitoso,
                motivo_fallo = Limitar(motivo, 200)
            };

            conexion.Sesiones!.Add(sesion);
            await conexion.SaveChangesAsync();
            return sesion.id;
        }

        private static string? Limitar(string? texto, int maximo)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            var limpio = texto.Trim();
            return limpio.Length <= maximo ? limpio : limpio[..maximo];
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaSimple>> CerrarSesion(
            [FromBody] PedidoCerrarSesion pedido)
        {
            if (pedido.sesion_id <= 0)
                return BadRequest(new RespuestaSimple { mensaje = "Sesión no válida." });

            var sesion = await conexion.Sesiones!
                .AsTracking()
                .FirstOrDefaultAsync(x => x.id == pedido.sesion_id);

            if (sesion is null)
                return NotFound(new RespuestaSimple { mensaje = "La sesión no existe." });

            sesion.fecha_fin = DateTime.Now;
            await conexion.SaveChangesAsync();

            return new RespuestaSimple { mensaje = "Sesión cerrada." };
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaSimple>> CrearUsuario([FromBody] PedidoCrearUsuario pedido)
        {
            if (string.IsNullOrWhiteSpace(pedido.nombre_usuario)
                || string.IsNullOrWhiteSpace(pedido.correo)
                || string.IsNullOrWhiteSpace(pedido.clave))
            {
                return BadRequest(new RespuestaSimple
                {
                    mensaje = "Usuario, correo y clave son obligatorios."
                });
            }

            var repetido = await conexion.Usuarios!
                .AnyAsync(x => x.nombre_usuario == pedido.nombre_usuario || x.correo == pedido.correo);

            if (repetido)
            {
                return Conflict(new RespuestaSimple
                {
                    mensaje = "Ese usuario o correo ya está registrado."
                });
            }

            var rolExiste = await conexion.Roles!.AnyAsync(x => x.id == pedido.rol);
            var personaExiste = await conexion.Personas!.AnyAsync(x => x.id == pedido.persona);
            if (!rolExiste || !personaExiste)
            {
                return BadRequest(new RespuestaSimple
                {
                    mensaje = "Seleccione un rol y una persona válidos."
                });
            }

            var sal = RandomNumberGenerator.GetBytes(16);
            var entidad = new Usuarios
            {
                nombre_usuario = pedido.nombre_usuario,
                correo = pedido.correo,
                password_salt = sal,
                password_hash = ClaveSeguridad.Hash(pedido.clave, sal),
                rol = pedido.rol,
                persona = pedido.persona,
                activo = pedido.activo,
                fecha_creacion = DateTime.Now
            };

            conexion.Usuarios!.Add(entidad);
            await conexion.SaveChangesAsync();

            return new RespuestaSimple { mensaje = "Usuario creado correctamente." };
        }

        [HttpPost]
        public override async Task<ActionResult<Usuarios>> Actualizar([FromBody] Usuarios entidad)
        {
            var nombre = entidad.nombre_usuario?.Trim();
            var correo = entidad.correo?.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo))
            {
                return BadRequest(new RespuestaSimple
                {
                    mensaje = "Usuario y correo son obligatorios."
                });
            }

            if (nombre.Length > 50 || correo.Length > 100)
            {
                return BadRequest(new RespuestaSimple
                {
                    mensaje = "El usuario admite 50 caracteres y el correo 100."
                });
            }

            var actual = await conexion.Usuarios!
                .AsTracking()
                .FirstOrDefaultAsync(x => x.id == entidad.id);

            if (actual is null)
            {
                return NotFound(new RespuestaSimple
                {
                    mensaje = "El usuario que intenta actualizar no existe."
                });
            }

            var repetido = await conexion.Usuarios!
                .AnyAsync(x => x.id != entidad.id
                    && (x.nombre_usuario == nombre || x.correo == correo));

            if (repetido)
            {
                return Conflict(new RespuestaSimple
                {
                    mensaje = "Ese usuario o correo ya está registrado."
                });
            }

            var rolExiste = await conexion.Roles!.AnyAsync(x => x.id == entidad.rol);
            var personaExiste = await conexion.Personas!.AnyAsync(x => x.id == entidad.persona);

            if (!rolExiste || !personaExiste)
            {
                return BadRequest(new RespuestaSimple
                {
                    mensaje = "Seleccione un rol y una persona válidos."
                });
            }

            actual.nombre_usuario = nombre;
            actual.correo = correo;
            actual.rol = entidad.rol;
            actual.persona = entidad.persona;
            actual.activo = entidad.activo;

            try
            {
                await conexion.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new RespuestaSimple
                {
                    mensaje = "No se pudo guardar el usuario. Revise los datos e intente de nuevo."
                });
            }

            return actual;
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
