using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Estanterias
    {
        [Key] public int id { get; set; }
        public string? codigo { get; set; }
        public int numero { get; set; }
        public int capacidad { get; set; }
        public int seccion { get; set; }

        [ForeignKey("seccion")] public Secciones? _seccion { get; set; }

        public List<Ubicaciones>? ubicaciones { get; set; }
    }
}
