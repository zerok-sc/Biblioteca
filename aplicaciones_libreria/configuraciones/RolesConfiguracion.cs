using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones_libreria.configuraciones
{
    public class RolesConfiguracion : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.nombre).IsRequired().HasMaxLength(50);
            builder.Property(x => x.codigo).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.codigo).IsUnique();
            builder.Property(x => x.descripcion).HasMaxLength(200);
            builder.Property(x => x.activo).HasDefaultValue(true);
            builder.Property(x => x.fecha_creacion).HasColumnType("smalldatetime");
        }
    }
}
