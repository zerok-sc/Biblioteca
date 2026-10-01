using aplicaciones_libreria.entidades;

namespace presentacion_pruebas
{
    [TestClass]
    public class EstadosPruebas : PruebaBase
    {
        private Estados? entidad;

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
            this.entidad = Guardar(c => c.Estados, new Estados
            {
                nombre = "Estado de Prueba",
                codigo = Unico("EST"),
                descripcion = "Estado creado por la prueba",
                fecha_creacion = new DateTime(2025, 1, 15, 8, 30, 0)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del estado.");

            var guardado = NuevaConexion().Estados!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El estado no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual("Estado de Prueba", guardado.nombre);
            Assert.AreEqual(new DateTime(2025, 1, 15, 8, 30, 0), guardado.fecha_creacion);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Estados!.ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de estados está vacía.");
            Assert.IsTrue(lista.Any(x => x.id == this.entidad!.id), "El estado insertado no aparece en la consulta.");
        }

        private void Actualizar()
        {
            this.entidad!.descripcion = "Descripción modificada";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Estados!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Descripción modificada", actualizado.descripcion, "No se actualizó la descripción.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Estados, this.entidad!);

            var borrado = NuevaConexion().Estados!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El estado no fue eliminado.");
        }
    }
}
