using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace aplicaciones_libreria.interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }

        DbSet<Personas>? Personas { get; set; }
        DbSet<Empleados>? Empleados { get; set; }
        DbSet<Salarios>? Salarios { get; set; }
        DbSet<Estados>? Estados { get; set; }
        DbSet<Editoriales>? Editoriales { get; set; }
        DbSet<Idiomas>? Idiomas { get; set; }
        DbSet<Libros>? Libros { get; set; }
        DbSet<Autores>? Autores { get; set; }
        DbSet<Libros_Autores>? Libros_Autores { get; set; }
        DbSet<Categorias>? Categorias { get; set; }
        DbSet<Libros_Categorias>? Libros_Categorias { get; set; }
        DbSet<Secciones>? Secciones { get; set; }
        DbSet<Estanterias>? Estanterias { get; set; }
        DbSet<Ubicaciones>? Ubicaciones { get; set; }
        DbSet<Ejemplares>? Ejemplares { get; set; }
        DbSet<Prestamos>? Prestamos { get; set; }
        DbSet<Prestamos_Libros>? Prestamos_Libros { get; set; }
        DbSet<Multas>? Multas { get; set; }
        DbSet<Proveedores>? Proveedores { get; set; }
        DbSet<Compras>? Compras { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
