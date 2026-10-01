using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class MultasPruebas : PruebaBase
    {
        private Prestamos_Libros? prestamoLibro;
        private Estados? estado;
        private Multas? entidad;

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
            this.prestamoLibro = CrearPrestamoLibro();
            this.estado = CrearEstado();

            this.entidad = Guardar(c => c.Multas, new Multas
            {
                valor = 5000.00m,
                fecha = new DateTime(2025, 4, 10, 9, 0, 0),
                prestamo_libro = this.prestamoLibro.id,
                estado = this.estado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Multas!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(5000.00m, guardado.valor);
            Assert.AreEqual(this.prestamoLibro.id, guardado.prestamo_libro);
            Assert.AreEqual(this.estado.id, guardado.estado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Multas!
                .Include(x => x._prestamo_libro)
                .Include(x => x._estado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._prestamo_libro, "No se cargó la relación _prestamo_libro.");
            Assert.AreEqual(this.prestamoLibro!.id, encontrado._prestamo_libro.id);
            Assert.IsNotNull(encontrado._estado, "No se cargó la relación _estado.");
            Assert.AreEqual(this.estado!.id, encontrado._estado.id);
        }

        private void Actualizar()
        {
            this.entidad!.valor = 8000.50m;
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Multas!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(8000.50m, actualizado.valor, "No se actualizó el valor de la multa.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Multas, this.entidad!);

            var borrado = NuevaConexion().Multas!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
