using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Salarios
    {
        [Key] public int id { get; set; }
        public int empleado { get; set; }
        [Column(TypeName = "decimal(10,2)")] public decimal valor { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string? observaciones { get; set; }

        [ForeignKey("empleado")] public Empleados? _empleado { get; set; }
    }
}
