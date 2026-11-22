namespace presentacion_libreria.interfaces
{
    public interface IComunicaciones
    {
        Task<T> Ejecutar<T>(string url, object? entidad = null);
    }
}
