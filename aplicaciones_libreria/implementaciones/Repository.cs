using System.Linq.Expressions;
using aplicaciones_libreria.interfaces;
using Microsoft.EntityFrameworkCore;

namespace aplicaciones_libreria.implementaciones
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IConexion conexion;

        public Repository(IConexion conexion)
        {
            this.conexion = conexion;
        }

        public IQueryable<T> Consultar() => conexion.Set<T>();

        public async Task<IReadOnlyList<T>> ObtenerTodoAsync(CancellationToken cancelacion = default)
        {
            return await conexion.Set<T>().ToListAsync(cancelacion);
        }

        public async Task<T?> ObtenerPorIdAsync(int id, CancellationToken cancelacion = default)
        {
            return await conexion.Set<T>()
                .FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id, cancelacion);
        }

        public async Task<IReadOnlyList<T>> ObtenerPorAsync(
            Expression<Func<T, bool>> predicado,
            CancellationToken cancelacion = default)
        {
            return await conexion.Set<T>().Where(predicado).ToListAsync(cancelacion);
        }

        public async Task<T> AgregarAsync(T entidad, CancellationToken cancelacion = default)
        {
            conexion.Set<T>().Add(entidad);
            await conexion.SaveChangesAsync(cancelacion);
            return entidad;
        }

        public async Task ActualizarAsync(T entidad, CancellationToken cancelacion = default)
        {
            conexion.Entry(entidad).State = EntityState.Modified;
            await conexion.SaveChangesAsync(cancelacion);
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken cancelacion = default)
        {
            var entidad = await ObtenerPorIdAsync(id, cancelacion);
            if (entidad is null)
            {
                return false;
            }

            conexion.Set<T>().Remove(entidad);
            try
            {
                await conexion.SaveChangesAsync(cancelacion);
            }
            catch (DbUpdateException)
            {
                conexion.Entry(entidad).State = EntityState.Unchanged;
                throw new InvalidOperationException(
                    "No se puede borrar: el registro tiene datos relacionados.");
            }

            return true;
        }

        public Task<int> GuardarCambiosAsync(CancellationToken cancelacion = default)
        {
            return conexion.SaveChangesAsync(cancelacion);
        }
    }
}
