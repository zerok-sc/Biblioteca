using aplicaciones_libreria.entidades;
using presentacion_libreria.interfaces;

namespace presentacion_libreria.implementaciones
{
    public class Presentacion<T> : IPresentacion<T> where T : class, IEntidad, new()
    {
        private readonly IComunicaciones comunicaciones;
        private readonly string controlador = typeof(T).Name;

        public Presentacion(IComunicaciones comunicaciones)
        {
            this.comunicaciones = comunicaciones;
        }

        public Task<List<T>> Consultar()
        {
            return comunicaciones.Ejecutar<List<T>>($"{controlador}/Consultar");
        }

        public Task<List<T>> Buscar(T entidad)
        {
            return comunicaciones.Ejecutar<List<T>>($"{controlador}/Buscar", entidad);
        }

        public Task<T> Insertar(T entidad)
        {
            return comunicaciones.Ejecutar<T>($"{controlador}/Insertar", entidad);
        }

        public Task<T> Actualizar(T entidad)
        {
            return comunicaciones.Ejecutar<T>($"{controlador}/Actualizar", entidad);
        }

        public Task<T> Borrar(T entidad)
        {
            return comunicaciones.Ejecutar<T>($"{controlador}/Borrar", entidad);
        }
    }
}
