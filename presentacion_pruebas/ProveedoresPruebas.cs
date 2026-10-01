using aplicaciones_libreria.entidades;

namespace presentacion_pruebas
{
    [TestClass]
    public class ProveedoresPruebas : PruebaBase
    {
        private Proveedores? entidad;

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
            this.entidad = Guardar(c => c.Proveedores, new Proveedores
            {
                nombre = "Proveedor de Prueba",
                nit = Unico("NIT"),
                telefono = "6042222222",
                correo = "proveedor@prueba.com"
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del proveedor.");

            var guardado = NuevaConexion().Proveedores!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El proveedor no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.nit, guardado.nit);
            Assert.AreEqual("Proveedor de Prueba", guardado.nombre);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Proveedores!.ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de proveedores está vacía.");
            Assert.IsTrue(lista.Any(x => x.id == this.entidad!.id), "El proveedor insertado no aparece en la consulta.");
        }

        private void Actualizar()
        {
            this.entidad!.telefono = "6049999999";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Proveedores!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("6049999999", actualizado.telefono, "No se actualizó el teléfono.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Proveedores, this.entidad!);

            var borrado = NuevaConexion().Proveedores!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El proveedor no fue eliminado.");
        }
    }
}
