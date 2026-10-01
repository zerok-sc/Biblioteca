using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Secciones
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public string? ubicacion { get; set; }
        public int estado { get; set; }

        [ForeignKey("estado")] public Estados? _estado { get; set; }

        public List<Estanterias>? estanterias { get; set; }
    }
}
