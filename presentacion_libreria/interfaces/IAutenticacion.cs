using aplicaciones_libreria.nucleo;

namespace presentacion_libreria.interfaces
{
    public interface IAutenticacion
    {
        Task<DatosSesion?> IniciarSesion(
            string usuario,
            string clave,
            string? direccionIp = null,
            string? agenteUsuario = null);
        Task CerrarSesion(int sesionId);
        Task<string> CrearUsuario(PedidoCrearUsuario pedido);
        Task<string?> ObtenerFoto(int id);
        Task CambiarFoto(int id, byte[] imagen, string tipo);
    }
}
