using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class Libros_CategoriasPruebas : PruebaBase
    {
        private Libros? libro;
        private Categorias? categoria;
        private Estados? estado;
        private Libros_Categorias? entidad;

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
            this.libro = CrearLibro();
            this.categoria = CrearCategoria();
            this.estado = CrearEstado();

            this.entidad = Guardar(c => c.Libros_Categorias, new Libros_Categorias
            {
                libro = this.libro.id,
                categoria = this.categoria.id,
                fecha_asignacion = new DateTime(2025, 2, 1, 10, 0, 0),
                estado = this.estado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de libros_categorias.");

            var guardado = NuevaConexion().Libros_Categorias!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.libro.id, guardado.libro);
            Assert.AreEqual(this.categoria.id, guardado.categoria);
            Assert.AreEqual(this.estado.id, guardado.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Libros_Categorias!
                .Include(x => x._libro)
                .Include(x => x._categoria)
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de libros_categorias está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._libro, "No se cargó la relación con libros.");
            Assert.IsNotNull(encontrado._categoria, "No se cargó la relación con categorías.");
            Assert.IsNotNull(encontrado._estado, "No se cargó la relación con estados.");
        }

        private void Actualizar()
        {
            var otroEstado = CrearEstado();
            this.entidad!.estado = otroEstado.id;
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Libros_Categorias!.First(x => x.id == this.entidad.id);
            Assert.AreEqual(otroEstado.id, actualizado.estado, "No se actualizó el estado.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Libros_Categorias, this.entidad!);

            var borrado = NuevaConexion().Libros_Categorias!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
