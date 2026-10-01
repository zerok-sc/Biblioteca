using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class Prestamos_LibrosPruebas : PruebaBase
    {
        private Prestamos? prestamo;
        private Ejemplares? ejemplar;
        private Estados? estado;
        private Prestamos_Libros? entidad;

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
            this.prestamo = CrearPrestamo();
            this.ejemplar = CrearEjemplar();
            this.estado = CrearEstado();

            this.entidad = Guardar(c => c.Prestamos_Libros, new Prestamos_Libros
            {
                prestamo = this.prestamo.id,
                ejemplar = this.ejemplar.id,
                estado = this.estado.id,
                fecha = new DateTime(2025, 3, 25, 9, 0, 0)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Prestamos_Libros!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.prestamo.id, guardado.prestamo);
            Assert.AreEqual(this.ejemplar.id, guardado.ejemplar);
            Assert.AreEqual(this.estado.id, guardado.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Prestamos_Libros!
                .Include(x => x._prestamo)
                .Include(x => x._ejemplar)
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._prestamo, "No se cargó la relación _prestamo.");
            Assert.AreEqual(this.prestamo!.id, encontrado._prestamo.id);
            Assert.IsNotNull(encontrado._ejemplar, "No se cargó la relación _ejemplar.");
            Assert.AreEqual(this.ejemplar!.id, encontrado._ejemplar.id);
            Assert.IsNotNull(encontrado._estado, "No se cargó la relación _estado.");
            Assert.AreEqual(this.estado!.id, encontrado._estado.id);
        }

        private void Actualizar()
        {
            var otroEstado = CrearEstado();
            this.entidad!.estado = otroEstado.id;
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Prestamos_Libros!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(otroEstado.id, actualizado.estado, "No se actualizó el estado.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Prestamos_Libros, this.entidad!);

            var borrado = NuevaConexion().Prestamos_Libros!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
