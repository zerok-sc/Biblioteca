using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Idiomas
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public string? descripcion { get; set; }
        public int estado { get; set; }

        [ForeignKey("estado")] public Estados? _estado { get; set; }

        public List<Libros>? libros { get; set; }
    }
}
