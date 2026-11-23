using aplicaciones_libreria.entidades;

namespace presentacion_libreria.interfaces
{
    // Canal exclusivo para escribir auditorías. La API no expone un CRUD genérico
    // de Auditorías: solo permite append a través de Registrar.
    public interface IAuditoriaPresentacion
    {
        Task Registrar(Auditorias auditoria);
    }
}
