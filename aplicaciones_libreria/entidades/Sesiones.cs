using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Sesiones : IEntidad
    {
        [Key] public int id { get; set; }
        public int usuario { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string? direccion_ip { get; set; }
        public string? agente_usuario { get; set; }
        public bool exitoso { get; set; }
        public string? motivo_fallo { get; set; }

        [ForeignKey("usuario")] public Usuarios? _usuario { get; set; }
    }
}
