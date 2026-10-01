using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class EstanteriasPruebas : PruebaBase
    {
        private Secciones? seccion;
        private Estanterias? entidad;

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
            this.seccion = CrearSeccion();

            this.entidad = Guardar(c => c.Estanterias, new Estanterias
            {
                codigo = Unico("ESTAN"),
                numero = 1,
                capacidad = 100,
                seccion = this.seccion.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de la estantería.");

            var guardada = NuevaConexion().Estanterias!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardada, "La estantería no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardada.codigo);
            Assert.AreEqual(100, guardada.capacidad);
            Assert.AreEqual(this.seccion.id, guardada.seccion);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Estanterias!
                .Include(x => x._seccion)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de estanterías está vacía.");

            var encontrada = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrada, "La estantería insertada no aparece en la consulta.");
            Assert.IsNotNull(encontrada._seccion, "No se cargó la relación con secciones.");
            Assert.AreEqual(this.seccion!.id, encontrada._seccion.id);
        }

        private void Actualizar()
        {
            this.entidad!.capacidad = 150;
            Modificar(this.entidad);

            var actualizada = NuevaConexion().Estanterias!.First(x => x.id == this.entidad.id);
            Assert.AreEqual(150, actualizada.capacidad, "No se actualizó la capacidad.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Estanterias, this.entidad!);

            var borrada = NuevaConexion().Estanterias!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrada, "La estantería no fue eliminada.");
        }
    }
}
