using System.ComponentModel.DataAnnotations;

namespace aplicaciones_libreria.entidades
{
    public class Auditorias : IEntidad
    {
        [Key] public int id { get; set; }
        public string entidad { get; set; } = string.Empty;
        public int entidad_id { get; set; }
        public string operacion { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public string? valores_anteriores { get; set; }
        public string? valores_nuevos { get; set; }
        public string? nombre_usuario { get; set; }
        public string? direccion_ip { get; set; }
    }
}
