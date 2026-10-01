using aplicaciones_libreria.entidades;

namespace presentacion_pruebas
{
    [TestClass]
    public class EditorialesPruebas : PruebaBase
    {
        private Editoriales? entidad;

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
            this.entidad = Guardar(c => c.Editoriales, new Editoriales
            {
                nombre = "Editorial de Prueba",
                pais = "Colombia",
                telefono = "6041234567",
                correo = "editorial@prueba.com"
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de la editorial.");

            var guardada = NuevaConexion().Editoriales!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardada, "La editorial no quedó en la base de datos.");
            Assert.AreEqual("Editorial de Prueba", guardada.nombre);
            Assert.AreEqual("Colombia", guardada.pais);
            Assert.AreEqual("editorial@prueba.com", guardada.correo);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Editoriales!.ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de editoriales está vacía.");
            Assert.IsTrue(lista.Any(x => x.id == this.entidad!.id), "La editorial insertada no aparece en la consulta.");
        }

        private void Actualizar()
        {
            this.entidad!.pais = "México";
            Modificar(this.entidad);

            var actualizada = NuevaConexion().Editoriales!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("México", actualizada.pais, "No se actualizó el país.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Editoriales, this.entidad!);

            var borrada = NuevaConexion().Editoriales!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrada, "La editorial no fue eliminada.");
        }
    }
}
