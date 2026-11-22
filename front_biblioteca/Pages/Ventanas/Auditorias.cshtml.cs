using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    public class AuditoriasModel : PaginaBase<Auditorias>
    {
        public AuditoriasModel(IPresentacion<Auditorias> presentacion) : base(presentacion)
        {
        }
    }
}
