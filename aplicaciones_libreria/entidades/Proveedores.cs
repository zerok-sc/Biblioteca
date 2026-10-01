using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Proveedores
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? nit { get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }

        public List<Compras>? compras { get; set; }
    }
}
