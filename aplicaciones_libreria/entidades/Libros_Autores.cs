using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Libros_Autores
    {
        [Key] public int id { get; set; }
        public int libro { get; set; }
        public int autor { get; set; }
        public string? tipo_participacion { get; set; }
        public DateTime fecha_registro { get; set; }

        [ForeignKey("libro")] public Libros? _libro { get; set; }
        [ForeignKey("autor")] public Autores? _autor { get; set; }
    }
}
