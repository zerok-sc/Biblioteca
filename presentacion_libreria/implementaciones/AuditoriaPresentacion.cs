using aplicaciones_libreria.entidades;
using presentacion_libreria.interfaces;

namespace presentacion_libreria.implementaciones
{
    public class AuditoriaPresentacion : IAuditoriaPresentacion
    {
        private readonly IComunicaciones comunicaciones;

        public AuditoriaPresentacion(IComunicaciones comunicaciones)
        {
            this.comunicaciones = comunicaciones;
        }

        public Task Registrar(Auditorias auditoria) =>
            comunicaciones.Ejecutar<Auditorias>("Auditorias/Registrar", auditoria);
    }
}
