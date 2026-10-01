using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class SeccionesPruebas : PruebaBase
    {
        private Estados? estado;
        private Secciones? entidad;

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

            this.entidad = Guardar(c => c.Secciones, new Secciones
            {
                nombre = "Sección de Prueba",
                descripcion = "Sección creada por la prueba",
                ubicacion = "Piso 1",
                estado = this.estado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de la sección.");

            var guardada = NuevaConexion().Secciones!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardada, "La sección no quedó en la base de datos.");
            Assert.AreEqual("Sección de Prueba", guardada.nombre);
            Assert.AreEqual(this.estado.id, guardada.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Secciones!
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de secciones está vacía.");

            var encontrada = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrada, "La sección insertada no aparece en la consulta.");
            Assert.IsNotNull(encontrada._estado, "No se cargó la relación con estados.");
            Assert.AreEqual(this.estado!.id, encontrada._estado.id);
        }

        private void Actualizar()
        {
            this.entidad!.descripcion = "Descripción modificada";
            Modificar(this.entidad);

            var actualizada = NuevaConexion().Secciones!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Descripción modificada", actualizada.descripcion, "No se actualizó la descripción.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Secciones, this.entidad!);

            var borrada = NuevaConexion().Secciones!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrada, "La sección no fue eliminada.");
        }
    }
}
