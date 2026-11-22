using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones_libreria.configuraciones
{
    public class AuditoriasConfiguracion : IEntityTypeConfiguration<Auditorias>
    {
        public void Configure(EntityTypeBuilder<Auditorias> builder)
        {
            builder.ToTable("Auditorias");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.entidad).IsRequired().HasMaxLength(100);
            builder.Property(x => x.operacion).IsRequired().HasMaxLength(20);
            builder.Property(x => x.fecha).HasColumnType("smalldatetime").HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.valores_anteriores).HasColumnType("nvarchar(max)");
            builder.Property(x => x.valores_nuevos).HasColumnType("nvarchar(max)");
            builder.Property(x => x.nombre_usuario).HasMaxLength(50);
            builder.Property(x => x.direccion_ip).HasMaxLength(50);
            builder.HasIndex(x => new { x.entidad, x.entidad_id });
            builder.HasIndex(x => x.fecha);
        }
    }
}
