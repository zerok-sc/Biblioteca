using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Libros_Categorias
    {
        [Key] public int id { get; set; }
        public int libro { get; set; }
        public int categoria { get; set; }
        public DateTime fecha_asignacion { get; set; }
        public int estado { get; set; }

        [ForeignKey("libro")] public Libros? _libro { get; set; }
        [ForeignKey("categoria")] public Categorias? _categoria { get; set; }
        [ForeignKey("estado")] public Estados? _estado { get; set; }
    }
}
