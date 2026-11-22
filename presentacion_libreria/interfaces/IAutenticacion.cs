using aplicaciones_libreria.nucleo;

namespace presentacion_libreria.interfaces
{
    public interface IAutenticacion
    {
        Task<DatosSesion?> IniciarSesion(string usuario, string clave);
        Task<string?> ObtenerFoto(int id);
        Task CambiarFoto(int id, byte[] imagen, string tipo);
    }
}
