using aplicaciones_libreria.entidades;

namespace presentacion_pruebas
{
    [TestClass]
    public class AutoresPruebas : PruebaBase
    {
        private Autores? entidad;

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
            this.entidad = Guardar(c => c.Autores, new Autores
            {
                nombre = "Autor",
                apellido = "de Prueba",
                nacionalidad = "Colombiana",
                fecha_nacimiento = new DateTime(1950, 5, 20)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del autor.");

            var guardado = NuevaConexion().Autores!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El autor no quedó en la base de datos.");
            Assert.AreEqual("Autor", guardado.nombre);
            Assert.AreEqual("de Prueba", guardado.apellido);
            Assert.AreEqual<DateTime?>(new DateTime(1950, 5, 20), guardado.fecha_nacimiento);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Autores!.ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de autores está vacía.");
            Assert.IsTrue(lista.Any(x => x.id == this.entidad!.id), "El autor insertado no aparece en la consulta.");
        }

        private void Actualizar()
        {
            this.entidad!.nacionalidad = "Argentina";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Autores!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Argentina", actualizado.nacionalidad, "No se actualizó la nacionalidad.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Autores, this.entidad!);

            var borrado = NuevaConexion().Autores!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El autor no fue eliminado.");
        }
    }
}
