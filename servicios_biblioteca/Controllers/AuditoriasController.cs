using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace servicios_biblioteca.Controllers
{
    // Auditorías es un registro histórico: solo se consulta.
    // No hereda ControladorBase para no exponer Insertar/Actualizar/Borrar.
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuditoriasController : ControllerBase
    {
        private readonly IRepository<Auditorias> repositorio;

        public AuditoriasController(IRepository<Auditorias> repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpPost]
        public async Task<List<Auditorias>> Consultar()
        {
            return (await repositorio.ObtenerTodoAsync()).ToList();
        }

        [HttpPost]
        public async Task<List<Auditorias>> Buscar([FromBody] Auditorias entidad)
        {
            if (entidad.id > 0)
                return (await repositorio.ObtenerPorAsync(x => x.id == entidad.id)).ToList();

            return (await repositorio.ObtenerTodoAsync()).ToList();
        }

        // Append controlado: el sistema puede registrar auditorías, pero nadie
        // puede modificar ni borrar el histórico a través de la API.
        [HttpPost]
        public async Task<Auditorias> Registrar([FromBody] Auditorias entidad)
        {
            entidad.id = 0;
            entidad.fecha = DateTime.Now;
            return await repositorio.AgregarAsync(entidad);
        }
    }
}
