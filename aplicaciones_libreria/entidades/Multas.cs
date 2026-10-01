using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Multas
    {
        [Key] public int id { get; set; }
        [Column(TypeName = "decimal(10,2)")] public decimal valor { get; set; }
        public DateTime fecha { get; set; }
        public int prestamo_libro { get; set; }
        public int estado { get; set; }

        [ForeignKey("prestamo_libro")] public Prestamos_Libros? _prestamo_libro { get; set; }
        [ForeignKey("estado")] public Estados? _estado { get; set; }
    }
}
