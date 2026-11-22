using System.Linq.Expressions;

namespace aplicaciones_libreria.interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Consultar();
        Task<IReadOnlyList<T>> ObtenerTodoAsync(CancellationToken cancelacion = default);
        Task<T?> ObtenerPorIdAsync(int id, CancellationToken cancelacion = default);
        Task<IReadOnlyList<T>> ObtenerPorAsync(Expression<Func<T, bool>> predicado, CancellationToken cancelacion = default);
        Task<T> AgregarAsync(T entidad, CancellationToken cancelacion = default);
        Task ActualizarAsync(T entidad, CancellationToken cancelacion = default);
        Task<bool> EliminarAsync(int id, CancellationToken cancelacion = default);
        Task<int> GuardarCambiosAsync(CancellationToken cancelacion = default);
    }
}
