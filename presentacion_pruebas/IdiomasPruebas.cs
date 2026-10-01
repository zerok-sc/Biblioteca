using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class IdiomasPruebas : PruebaBase
    {
        private Estados? estado;
        private Idiomas? entidad;

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

            this.entidad = Guardar(c => c.Idiomas, new Idiomas
            {
                nombre = "Idioma de Prueba",
                codigo = Unico("IDI"),
                descripcion = "Idioma creado por la prueba",
                estado = this.estado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del idioma.");

            var guardado = NuevaConexion().Idiomas!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El idioma no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual(this.estado.id, guardado.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Idiomas!
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de idiomas está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El idioma insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._estado, "No se cargó la relación con estados.");
            Assert.AreEqual(this.estado!.id, encontrado._estado.id);
        }

        private void Actualizar()
        {
            this.entidad!.descripcion = "Descripción modificada";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Idiomas!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Descripción modificada", actualizado.descripcion, "No se actualizó la descripción.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Idiomas, this.entidad!);

            var borrado = NuevaConexion().Idiomas!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El idioma no fue eliminado.");
        }
    }
}
