using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class EjemplaresPruebas : PruebaBase
    {
        private Estados? estado;
        private Libros? libro;
        private Ubicaciones? ubicacion;
        private Ejemplares? entidad;

        [TestMethod]
        public void Ejecutar()
        {
            Insertar();
            Consultar();
            Actualizar();
            Borrar();
        }

        private void Insertar()
        {
            this.estado = CrearEstado();
            this.libro = CrearLibro();
            this.ubicacion = CrearUbicacion();

            this.entidad = Guardar(c => c.Ejemplares, new Ejemplares
            {
                codigo = Unico("EJM"),
                estado = this.estado.id,
                libro = this.libro.id,
                ubicacion = this.ubicacion.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Ejemplares!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual(this.estado.id, guardado.estado);
            Assert.AreEqual(this.libro.id, guardado.libro);
            Assert.AreEqual(this.ubicacion.id, guardado.ubicacion);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Ejemplares!
                .Include(x => x._estado)
                .Include(x => x._libro)
                .Include(x => x._ubicacion)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._estado, "No se cargó la relación _estado.");
            Assert.AreEqual(this.estado!.id, encontrado._estado.id);
            Assert.IsNotNull(encontrado._libro, "No se cargó la relación _libro.");
            Assert.AreEqual(this.libro!.id, encontrado._libro.id);
            Assert.IsNotNull(encontrado._ubicacion, "No se cargó la relación _ubicacion.");
            Assert.AreEqual(this.ubicacion!.id, encontrado._ubicacion.id);
        }

        private void Actualizar()
        {
            var otroEstado = CrearEstado();
            this.entidad!.estado = otroEstado.id;
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Ejemplares!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(otroEstado.id, actualizado.estado, "No se actualizó el estado.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Ejemplares, this.entidad!);

            var borrado = NuevaConexion().Ejemplares!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
