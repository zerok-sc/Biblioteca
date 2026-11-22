using aplicaciones_libreria.entidades;

namespace presentacion_libreria.interfaces
{
    public interface IPresentacion<T> where T : class, IEntidad, new()
    {
        Task<List<T>> Consultar();
        Task<List<T>> Buscar(T entidad);
        Task<T> Insertar(T entidad);
        Task<T> Actualizar(T entidad);
        Task<T> Borrar(T entidad);
    }
}
