using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Compras
    {
        [Key] public int id { get; set; }
        public DateTime fecha { get; set; }
        [Column(TypeName = "decimal(10,2)")] public decimal total { get; set; }
        public int proveedor { get; set; }
        public int empleado { get; set; }

        [ForeignKey("proveedor")] public Proveedores? _proveedor { get; set; }
        [ForeignKey("empleado")] public Empleados? _empleado { get; set; }
    }
}
