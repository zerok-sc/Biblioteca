using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace servicios_biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class ControladorBase<T> : ControllerBase where T : class, IEntidad, new()
    {
        private readonly IRepository<T> repositorio;

        protected ControladorBase(IRepository<T> repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpPost]
        public async Task<List<T>> Consultar()
        {
            return (await repositorio.ObtenerTodoAsync()).ToList();
        }

        [HttpPost]
        public async Task<List<T>> Buscar([FromBody] T entidad)
        {
            if (entidad.id > 0)
            {
                return (await repositorio.ObtenerPorAsync(x => x.id == entidad.id)).ToList();
            }

            return (await repositorio.ObtenerTodoAsync()).ToList();
        }

        [HttpPost]
        public async Task<T> Insertar([FromBody] T entidad)
        {
            return await repositorio.AgregarAsync(entidad);
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> Actualizar([FromBody] T entidad)
        {
            await repositorio.ActualizarAsync(entidad);
            return entidad;
        }

        [HttpPost]
        public async Task<T> Borrar([FromBody] T entidad)
        {
            var eliminado = await repositorio.EliminarAsync(entidad.id);
            if (!eliminado)
                throw new KeyNotFoundException($"No existe el registro con id {entidad.id}.");

            return entidad;
        }
    }
}
