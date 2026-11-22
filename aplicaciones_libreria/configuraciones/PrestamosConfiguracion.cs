using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones_libreria.configuraciones
{
    public class PrestamosConfiguracion : IEntityTypeConfiguration<Prestamos>
    {
        public void Configure(EntityTypeBuilder<Prestamos> builder)
        {
            builder.ToTable("Prestamos");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.codigo).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.codigo).IsUnique();
            builder.Property(x => x.fecha).HasColumnType("smalldatetime");
            builder.Property(x => x.fecha_inicio).HasColumnType("smalldatetime");
            builder.Property(x => x.fecha_fin).HasColumnType("smalldatetime");
            builder.Property(x => x.tiempo).HasDefaultValue(15);

            builder.HasOne(x => x._persona)
                .WithMany(x => x.prestamos)
                .HasForeignKey(x => x.persona)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x._empleado)
                .WithMany(x => x.prestamos)
                .HasForeignKey(x => x.empleado)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x._estado)
                .WithMany()
                .HasForeignKey(x => x.estado)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
