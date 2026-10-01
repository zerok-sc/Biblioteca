using aplicaciones_libreria.entidades;
using aplicaciones_libreria.implementaciones;
using aplicaciones_libreria.interfaces;
using aplicaciones_libreria.nucleo;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    /// <summary>
    /// Clase base de todas las pruebas. Ofrece:
    ///  - La conexion a la base de datos.
    ///  - Metodos Guardar / Modificar / Eliminar reutilizables.
    ///  - Metodos CrearXxx que insertan (y luego limpian) los registros "padre"
    ///    que una entidad necesita por sus llaves foraneas. Asi cada prueba es
    ///    independiente y no depende de los datos de Script.sql.
    /// </summary>
    public abstract class PruebaBase
    {
        protected static readonly string StringConexion =
            DatosGenerales.StringConexion();

        // Pila de acciones de limpieza: se ejecutan al reves (hijos antes que padres).
        private readonly Stack<Action> limpieza = new();

        protected static IConexion NuevaConexion()
        {
            IConexion conexion = new Conexion();
            conexion.StringConexion = StringConexion;
            return conexion;
        }

        /// <summary>Genera un codigo unico (las columnas codigo/cedula/nit son UNIQUE).</summary>
        protected static string Unico(string prefijo)
        {
            return prefijo + "-" + Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        // ------------------------------------------------------------------
        //  Operaciones genericas
        // ------------------------------------------------------------------
        protected T Guardar<T>(Func<IConexion, DbSet<T>?> tabla, T entidad) where T : class
        {
            var conexion = NuevaConexion();
            tabla(conexion)!.Add(entidad);
            conexion.SaveChanges();

            // Si la prueba falla a la mitad, igual se borra el registro.
            this.limpieza.Push(() =>
            {
                var c = NuevaConexion();
                tabla(c)!.Remove(entidad);
                c.SaveChanges();
            });
            return entidad;
        }

        protected static void Modificar<T>(T entidad) where T : class
        {
            var conexion = NuevaConexion();
            var entry = conexion.Entry(entidad);
            entry.State = EntityState.Modified;
            conexion.SaveChanges();
        }

        protected static void Eliminar<T>(Func<IConexion, DbSet<T>?> tabla, T entidad) where T : class
        {
            var conexion = NuevaConexion();
            tabla(conexion)!.Remove(entidad);
            conexion.SaveChanges();
        }

        [TestCleanup]
        public void Limpiar()
        {
            while (this.limpieza.Count > 0)
            {
                var accion = this.limpieza.Pop();
                try
                {
                    accion();
                }
                catch
                {
                    // El registro ya fue borrado por la propia prueba (paso "Borrar").
                }
            }
        }

        // ------------------------------------------------------------------
        //  Registros padre (cada uno crea, a su vez, los que necesita)
        // ------------------------------------------------------------------
        protected Personas CrearPersona()
        {
            return Guardar(c => c.Personas, new Personas
            {
                cedula = Unico("CED"),
                nombre = "Persona de apoyo",
                fecha_nacimiento = new DateTime(1990, 1, 1),
                telefono = "3000000000"
            });
        }

        protected Estados CrearEstado()
        {
            return Guardar(c => c.Estados, new Estados
            {
                nombre = "Estado de apoyo",
                codigo = Unico("EST"),
                descripcion = "Creado por las pruebas",
                fecha_creacion = new DateTime(2025, 1, 15, 8, 30, 0)
            });
        }

        protected Editoriales CrearEditorial()
        {
            return Guardar(c => c.Editoriales, new Editoriales
            {
                nombre = "Editorial de apoyo",
                pais = "Colombia",
                telefono = "6041234567",
                correo = "editorial@prueba.com"
            });
        }

        protected Autores CrearAutor()
        {
            return Guardar(c => c.Autores, new Autores
            {
                nombre = "Autor",
                apellido = "de Apoyo",
                nacionalidad = "Colombiana",
                fecha_nacimiento = new DateTime(1950, 5, 20)
            });
        }

        protected Proveedores CrearProveedor()
        {
            return Guardar(c => c.Proveedores, new Proveedores
            {
                nombre = "Proveedor de apoyo",
                nit = Unico("NIT"),
                telefono = "6042222222",
                correo = "proveedor@prueba.com"
            });
        }

        protected Empleados CrearEmpleado()
        {
            return Guardar(c => c.Empleados, new Empleados
            {
                persona = CrearPersona().id,
                cargo = "Bibliotecario",
                fecha_contratacion = new DateTime(2024, 2, 1)
            });
        }

        protected Idiomas CrearIdioma()
        {
            return Guardar(c => c.Idiomas, new Idiomas
            {
                nombre = "Idioma de apoyo",
                codigo = Unico("IDI"),
                descripcion = "Creado por las pruebas",
                estado = CrearEstado().id
            });
        }

        protected Libros CrearLibro()
        {
            return Guardar(c => c.Libros, new Libros
            {
                codigo = Unico("LIB"),
                nombre = "Libro de apoyo",
                editorial = CrearEditorial().id,
                idioma = CrearIdioma().id
            });
        }

        protected Categorias CrearCategoria()
        {
            return Guardar(c => c.Categorias, new Categorias
            {
                nombre = "Categoria de apoyo",
                descripcion = "Creada por las pruebas",
                codigo = Unico("CAT"),
                estado = CrearEstado().id
            });
        }

        protected Secciones CrearSeccion()
        {
            return Guardar(c => c.Secciones, new Secciones
            {
                nombre = "Seccion de apoyo",
                descripcion = "Creada por las pruebas",
                ubicacion = "Piso 1",
                estado = CrearEstado().id
            });
        }

        protected Estanterias CrearEstanteria()
        {
            return Guardar(c => c.Estanterias, new Estanterias
            {
                codigo = Unico("ESTAN"),
                numero = 1,
                capacidad = 100,
                seccion = CrearSeccion().id
            });
        }

        protected Ubicaciones CrearUbicacion()
        {
            return Guardar(c => c.Ubicaciones, new Ubicaciones
            {
                codigo = Unico("UBI"),
                piso = 1,
                descripcion = "Repisa de apoyo",
                estanteria = CrearEstanteria().id
            });
        }

        protected Ejemplares CrearEjemplar()
        {
            return Guardar(c => c.Ejemplares, new Ejemplares
            {
                codigo = Unico("EJM"),
                estado = CrearEstado().id,
                libro = CrearLibro().id,
                ubicacion = CrearUbicacion().id
            });
        }

        protected Prestamos CrearPrestamo()
        {
            return Guardar(c => c.Prestamos, new Prestamos
            {
                codigo = Unico("PRES"),
                persona = CrearPersona().id,
                empleado = CrearEmpleado().id,
                fecha = new DateTime(2025, 3, 10, 9, 0, 0)
            });
        }

        protected Prestamos_Libros CrearPrestamoLibro()
        {
            return Guardar(c => c.Prestamos_Libros, new Prestamos_Libros
            {
                prestamo = CrearPrestamo().id,
                ejemplar = CrearEjemplar().id,
                estado = CrearEstado().id,
                fecha = new DateTime(2025, 3, 25, 9, 0, 0)
            });
        }
    }
}
