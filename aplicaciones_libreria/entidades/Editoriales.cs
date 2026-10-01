using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Editoriales
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? pais { get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }

        public List<Libros>? libros { get; set; }
    }
}
