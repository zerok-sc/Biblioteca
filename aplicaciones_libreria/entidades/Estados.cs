using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Estados
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public string? descripcion { get; set; }
        public DateTime fecha_creacion { get; set; }

        public List<Idiomas>? idiomas { get; set; }
        public List<Categorias>? categorias { get; set; }
        public List<Libros_Categorias>? libros_categorias { get; set; }
        public List<Secciones>? secciones { get; set; }
        public List<Ejemplares>? ejemplares { get; set; }
        public List<Prestamos_Libros>? prestamos_libros { get; set; }
        public List<Multas>? multas { get; set; }
    }
}
