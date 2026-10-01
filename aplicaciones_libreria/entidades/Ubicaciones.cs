using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Ubicaciones
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public int piso { get; set; }
        public string? descripcion { get; set; }
        public int estanteria { get; set; }

        [ForeignKey("estanteria")] public Estanterias? _estanteria { get; set; }

        public List<Ejemplares>? ejemplares { get; set; }
    }
}
