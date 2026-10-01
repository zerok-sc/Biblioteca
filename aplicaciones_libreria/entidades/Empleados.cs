using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Empleados
    {
        [Key] public int id { get; set; }
        public int persona { get; set; }
        public string? cargo { get; set; }
        public DateTime fecha_contratacion { get; set; }

        [ForeignKey("persona")] public Personas? _persona { get; set; }

        public List<Salarios>? salarios { get; set; }
        public List<Prestamos>? prestamos { get; set; }
        public List<Compras>? compras { get; set; }
    }
}
