using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aplicaciones_libreria.entidades
{
    public class Compras_Libros : IEntidad
    {
        [Key] public int id { get; set; }
        public int compra { get; set; }
        public int libro { get; set; }
        public int cantidad { get; set; }
        [Column(TypeName = "decimal(10,2)")] public decimal costo_unitario { get; set; }

        [ForeignKey("compra")] public Compras? _compra { get; set; }
        [ForeignKey("libro")] public Libros? _libro { get; set; }
    }
}
