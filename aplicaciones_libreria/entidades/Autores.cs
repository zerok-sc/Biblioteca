using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Autores
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? nacionalidad { get; set; }
        public DateTime? fecha_nacimiento { get; set; }

        public List<Libros_Autores>? libros_autores { get; set; }
    }
}
