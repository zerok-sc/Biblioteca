using aplicaciones_libreria.nucleo;
using presentacion_libreria.interfaces;

namespace presentacion_libreria.implementaciones
{
    public class Autenticacion : IAutenticacion
    {
        private readonly IComunicaciones comunicaciones;

        public Autenticacion(IComunicaciones comunicaciones)
        {
            this.comunicaciones = comunicaciones;
        }

        public async Task<DatosSesion?> IniciarSesion(string usuario, string clave)
        {
            try
            {
                return await comunicaciones.Ejecutar<DatosSesion?>(
                    "Usuarios/Login",
                    new PedidoLogin { usuario = usuario, clave = clave });
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                return null;
            }
        }

        public async Task<string?> ObtenerFoto(int id)
        {
            var datos = await comunicaciones.Ejecutar<DatosFoto>(
                "Usuarios/ObtenerFoto",
                new PedidoFoto { id = id });

            return datos?.imagen;
        }

        public Task CambiarFoto(int id, byte[] imagen, string tipo)
        {
            return comunicaciones.Ejecutar<RespuestaSimple>(
                "Usuarios/CambiarFoto",
                new PedidoFoto
                {
                    id = id,
                    imagen = Convert.ToBase64String(imagen),
                    tipo = tipo
                });
        }
    }
}
