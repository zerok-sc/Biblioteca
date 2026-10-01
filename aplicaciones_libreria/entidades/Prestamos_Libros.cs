using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Prestamos_Libros
    {
        [Key] public int id { get; set; }
        public int prestamo { get; set; }
        public int ejemplar { get; set; }
        public int estado { get; set; }
        public DateTime fecha { get; set; }

        [ForeignKey("prestamo")] public Prestamos? _prestamo { get; set; }
        [ForeignKey("ejemplar")] public Ejemplares? _ejemplar { get; set; }
        [ForeignKey("estado")] public Estados? _estado { get; set; }

        public List<Multas>? multas { get; set; }
    }
}
