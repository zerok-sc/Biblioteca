using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Personas
    {
        [Key] public int id { get; set; }
        public string? cedula { get; set; }
        public string? nombre { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public string? telefono { get; set; }

        public List<Empleados>? empleados { get; set; }
        public List<Prestamos>? prestamos { get; set; }
    }
}
