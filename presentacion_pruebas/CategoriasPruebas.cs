using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class CategoriasPruebas : PruebaBase
    {
        private Estados? estado;
        private Categorias? entidad;

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

            this.entidad = Guardar(c => c.Categorias, new Categorias
            {
                nombre = "Categoría de Prueba",
                descripcion = "Categoría creada por la prueba",
                codigo = Unico("CAT"),
                estado = this.estado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de la categoría.");

            var guardada = NuevaConexion().Categorias!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardada, "La categoría no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardada.codigo);
            Assert.AreEqual(this.estado.id, guardada.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Categorias!
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de categorías está vacía.");

            var encontrada = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrada, "La categoría insertada no aparece en la consulta.");
            Assert.IsNotNull(encontrada._estado, "No se cargó la relación con estados.");
            Assert.AreEqual(this.estado!.id, encontrada._estado.id);
        }

        private void Actualizar()
        {
            this.entidad!.nombre = "Categoría Modificada";
            Modificar(this.entidad);

            var actualizada = NuevaConexion().Categorias!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Categoría Modificada", actualizada.nombre, "No se actualizó el nombre.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Categorias, this.entidad!);

            var borrada = NuevaConexion().Categorias!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrada, "La categoría no fue eliminada.");
        }
    }
}
