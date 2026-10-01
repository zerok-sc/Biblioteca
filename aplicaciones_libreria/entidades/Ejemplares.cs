using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Ejemplares
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public int estado { get; set; }
        public int libro { get; set; }
        public int ubicacion { get; set; }

        [ForeignKey("estado")] public Estados? _estado { get; set; }
        [ForeignKey("libro")] public Libros? _libro { get; set; }
        [ForeignKey("ubicacion")] public Ubicaciones? _ubicacion { get; set; }

        public List<Prestamos_Libros>? prestamos_libros { get; set; }
    }
}
