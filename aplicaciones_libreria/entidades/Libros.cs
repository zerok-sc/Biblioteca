using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Libros
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public string? nombre { get; set; }
        public int editorial { get; set; }
        public int idioma { get; set; }

        [ForeignKey("editorial")] public Editoriales? _editorial { get; set; }
        [ForeignKey("idioma")] public Idiomas? _idioma { get; set; }

        public List<Libros_Autores>? libros_autores { get; set; }
        public List<Libros_Categorias>? libros_categorias { get; set; }
        public List<Ejemplares>? ejemplares { get; set; }
    }
}
