using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;
using Microsoft.EntityFrameworkCore;

namespace aplicaciones_libreria.implementaciones
{
    public class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Personas>? Personas { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Salarios>? Salarios { get; set; }
        public DbSet<Estados>? Estados { get; set; }
        public DbSet<Editoriales>? Editoriales { get; set; }
        public DbSet<Idiomas>? Idiomas { get; set; }
        public DbSet<Libros>? Libros { get; set; }
        public DbSet<Autores>? Autores { get; set; }
        public DbSet<Libros_Autores>? Libros_Autores { get; set; }
        public DbSet<Categorias>? Categorias { get; set; }
        public DbSet<Libros_Categorias>? Libros_Categorias { get; set; }
        public DbSet<Secciones>? Secciones { get; set; }
        public DbSet<Estanterias>? Estanterias { get; set; }
        public DbSet<Ubicaciones>? Ubicaciones { get; set; }
        public DbSet<Ejemplares>? Ejemplares { get; set; }
        public DbSet<Prestamos>? Prestamos { get; set; }
        public DbSet<Prestamos_Libros>? Prestamos_Libros { get; set; }
        public DbSet<Multas>? Multas { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Compras>? Compras { get; set; }
    }
}
