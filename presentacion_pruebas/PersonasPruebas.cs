using aplicaciones_libreria.entidades;

namespace presentacion_pruebas
{
    [TestClass]
    public class PersonasPruebas : PruebaBase
    {
        private Personas? entidad;

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
            this.entidad = Guardar(c => c.Personas, new Personas
            {
                cedula = Unico("CED"),
                nombre = "Persona de Prueba",
                fecha_nacimiento = new DateTime(2000, 1, 1),
                telefono = "3000000000"
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de la persona.");

            var guardada = NuevaConexion().Personas!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardada, "La persona no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.cedula, guardada.cedula);
            Assert.AreEqual("Persona de Prueba", guardada.nombre);
            Assert.AreEqual<DateTime?>(new DateTime(2000, 1, 1), guardada.fecha_nacimiento);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Personas!.ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de personas está vacía.");
            Assert.IsTrue(lista.Any(x => x.id == this.entidad!.id), "La persona insertada no aparece en la consulta.");
        }

        private void Actualizar()
        {
            this.entidad!.telefono = "3111111111";
            Modificar(this.entidad);

            var actualizada = NuevaConexion().Personas!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("3111111111", actualizada.telefono, "No se actualizó el teléfono.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Personas, this.entidad!);

            var borrada = NuevaConexion().Personas!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrada, "La persona no fue eliminada.");
        }
    }
}
