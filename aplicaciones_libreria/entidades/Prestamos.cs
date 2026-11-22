using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Prestamos : IEntidad
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public int persona { get; set; }
        public int empleado { get; set; }
        public DateTime fecha { get; set; }
        public int tiempo { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int estado { get; set; }

        [ForeignKey("persona")] public Personas? _persona { get; set; }
        [ForeignKey("empleado")] public Empleados? _empleado { get; set; }
        [ForeignKey("estado")] public Estados? _estado { get; set; }

        public List<Prestamos_Libros>? prestamos_libros { get; set; }
    }
}
