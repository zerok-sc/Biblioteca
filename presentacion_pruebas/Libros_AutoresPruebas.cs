using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class Libros_AutoresPruebas : PruebaBase
    {
        private Libros? libro;
        private Autores? autor;
        private Libros_Autores? entidad;

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
            this.autor = CrearAutor();

            this.entidad = Guardar(c => c.Libros_Autores, new Libros_Autores
            {
                libro = this.libro.id,
                autor = this.autor.id,
                tipo_participacion = "Autor Principal",
                fecha_registro = new DateTime(2025, 2, 1, 10, 0, 0)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id de libros_autores.");

            var guardado = NuevaConexion().Libros_Autores!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.libro.id, guardado.libro);
            Assert.AreEqual(this.autor.id, guardado.autor);
            Assert.AreEqual("Autor Principal", guardado.tipo_participacion);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Libros_Autores!
                .Include(x => x._libro)
                .Include(x => x._autor)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de libros_autores está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._libro, "No se cargó la relación con libros.");
            Assert.IsNotNull(encontrado._autor, "No se cargó la relación con autores.");
            Assert.AreEqual(this.libro!.id, encontrado._libro.id);
            Assert.AreEqual(this.autor!.id, encontrado._autor.id);
        }

        private void Actualizar()
        {
            this.entidad!.tipo_participacion = "Coautor";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Libros_Autores!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Coautor", actualizado.tipo_participacion, "No se actualizó el tipo de participación.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Libros_Autores, this.entidad!);

            var borrado = NuevaConexion().Libros_Autores!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
