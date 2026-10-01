using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class UbicacionesPruebas : PruebaBase
    {
        private Estanterias? estanteria;
        private Ubicaciones? entidad;

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
            this.estanteria = CrearEstanteria();

            this.entidad = Guardar(c => c.Ubicaciones, new Ubicaciones
            {
                codigo = Unico("UBI"),
                piso = 1,
                descripcion = "Repisa de prueba",
                estanteria = this.estanteria.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Ubicaciones!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual(1, guardado.piso);
            Assert.AreEqual(this.estanteria.id, guardado.estanteria);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Ubicaciones!
                .Include(x => x._estanteria)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._estanteria, "No se cargó la relación _estanteria.");
            Assert.AreEqual(this.estanteria!.id, encontrado._estanteria.id);
        }

        private void Actualizar()
        {
            this.entidad!.piso = 2;
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Ubicaciones!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(2, actualizado.piso, "No se actualizó el piso.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Ubicaciones, this.entidad!);

            var borrado = NuevaConexion().Ubicaciones!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
