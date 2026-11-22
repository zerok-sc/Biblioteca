using System.ComponentModel.DataAnnotations;

namespace aplicaciones_libreria.entidades
{
    public class Roles : IEntidad
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion { get; set; }

        public List<Usuarios>? usuarios { get; set; }
    }
}
