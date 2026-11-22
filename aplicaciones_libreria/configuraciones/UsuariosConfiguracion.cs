using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones_libreria.configuraciones
{
    public class UsuariosConfiguracion : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedOnAdd();
            builder.Property(x => x.nombre_usuario).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.nombre_usuario).IsUnique();
            builder.Property(x => x.correo).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.correo).IsUnique();
            builder.Property(x => x.password_hash).IsRequired().HasColumnType("varbinary(32)");
            builder.Property(x => x.password_salt).IsRequired().HasColumnType("varbinary(16)");
            builder.Property(x => x.activo).HasDefaultValue(true);
            builder.Property(x => x.fecha_creacion).HasColumnType("smalldatetime");
            builder.Property(x => x.ultimo_acceso).HasColumnType("smalldatetime");

            builder.HasOne(x => x._rol)
                .WithMany(x => x.usuarios)
                .HasForeignKey(x => x.rol)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x._persona)
                .WithMany()
                .HasForeignKey(x => x.persona)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
