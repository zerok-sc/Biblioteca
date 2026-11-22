using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones_libreria.configuraciones
{
    public class SesionesConfiguracion : IEntityTypeConfiguration<Sesiones>
    {
        public void Configure(EntityTypeBuilder<Sesiones> builder)
        {
            builder.ToTable("Sesiones");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.fecha_inicio).HasColumnType("smalldatetime");
            builder.Property(x => x.fecha_fin).HasColumnType("smalldatetime");
            builder.Property(x => x.direccion_ip).HasMaxLength(50);
            builder.Property(x => x.agente_usuario).HasMaxLength(400);
            builder.Property(x => x.motivo_fallo).HasMaxLength(200);
            builder.HasIndex(x => new { x.usuario, x.fecha_inicio });

            builder.HasOne(x => x._usuario)
                .WithMany(x => x.sesiones)
                .HasForeignKey(x => x.usuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
