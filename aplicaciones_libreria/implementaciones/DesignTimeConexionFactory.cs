using aplicaciones_libreria.nucleo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace aplicaciones_libreria.implementaciones
{
    public class DesignTimeConexionFactory : IDesignTimeDbContextFactory<Conexion>
    {
        public Conexion CreateDbContext(string[] args)
        {
            var opciones = new DbContextOptionsBuilder<Conexion>()
                .UseSqlServer(DatosGenerales.StringConexion())
                .Options;

            return new Conexion(opciones);
        }
    }
}
