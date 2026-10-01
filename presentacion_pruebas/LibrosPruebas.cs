using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class LibrosPruebas : PruebaBase
    {
        private Editoriales? editorial;
        private Idiomas? idioma;
        private Libros? entidad;

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
            this.editorial = CrearEditorial();
            this.idioma = CrearIdioma();

            this.entidad = Guardar(c => c.Libros, new Libros
            {
                codigo = Unico("LIB"),
                nombre = "Libro de Prueba",
                editorial = this.editorial.id,
                idioma = this.idioma.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del libro.");

            var guardado = NuevaConexion().Libros!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El libro no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual(this.editorial.id, guardado.editorial);
            Assert.AreEqual(this.idioma.id, guardado.idioma);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Libros!
                .Include(x => x._editorial)
                .Include(x => x._idioma)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de libros está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El libro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._editorial, "No se cargó la relación con editoriales.");
            Assert.IsNotNull(encontrado._idioma, "No se cargó la relación con idiomas.");
            Assert.AreEqual(this.editorial!.id, encontrado._editorial.id);
            Assert.AreEqual(this.idioma!.id, encontrado._idioma.id);
        }

        private void Actualizar()
        {
            this.entidad!.nombre = "Libro Modificado";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Libros!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Libro Modificado", actualizado.nombre, "No se actualizó el nombre del libro.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Libros, this.entidad!);

            var borrado = NuevaConexion().Libros!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El libro no fue eliminado.");
        }
    }
}
