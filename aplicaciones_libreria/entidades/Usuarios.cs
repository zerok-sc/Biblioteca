using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Usuarios : IEntidad
    {
        [Key] public int id { get; set; }
        public string? nombre_usuario { get; set; }
        public string? correo { get; set; }
        public byte[] password_hash { get; set; } = Array.Empty<byte>();
        public byte[] password_salt { get; set; } = Array.Empty<byte>();
        public int rol { get; set; }
        public int persona { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? ultimo_acceso { get; set; }
        public byte[]? foto { get; set; }
        public string? foto_tipo { get; set; }

        [ForeignKey("rol")] public Roles? _rol { get; set; }
        [ForeignKey("persona")] public Personas? _persona { get; set; }

        public List<Sesiones>? sesiones { get; set; }
    }
}
