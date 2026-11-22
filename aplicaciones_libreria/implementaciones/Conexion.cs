using aplicaciones_libreria.configuraciones;
using aplicaciones_libreria.entidades;
using aplicaciones_libreria.interfaces;
using aplicaciones_libreria.nucleo;
using Microsoft.EntityFrameworkCore;

namespace aplicaciones_libreria.implementaciones
{
    public class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        public Conexion()
        {
        }

        public Conexion(DbContextOptions<Conexion> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(StringConexion ?? DatosGenerales.StringConexion());
            }

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Conexion).Assembly);

            foreach (var llaveForanea in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                llaveForanea.DeleteBehavior = DeleteBehavior.Restrict;
            }
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
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Sesiones>? Sesiones { get; set; }
        public DbSet<Auditorias>? Auditorias { get; set; }
    }
}
