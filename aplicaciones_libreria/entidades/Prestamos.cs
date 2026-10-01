using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Prestamos
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public int persona { get; set; }
        public int empleado { get; set; }
        public DateTime fecha { get; set; }

        [ForeignKey("persona")] public Personas? _persona { get; set; }
        [ForeignKey("empleado")] public Empleados? _empleado { get; set; }

        public List<Prestamos_Libros>? prestamos_libros { get; set; }
    }
}
