using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class SalariosPruebas : PruebaBase
    {
        private Empleados? empleado;
        private Salarios? entidad;

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
            this.empleado = CrearEmpleado();

            this.entidad = Guardar(c => c.Salarios, new Salarios
            {
                empleado = this.empleado.id,
                valor = 1500000.50m,
                fecha_inicio = new DateTime(2024, 1, 1),
                fecha_fin = null,
                observaciones = "Salario de prueba"
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del salario.");

            var guardado = NuevaConexion().Salarios!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El salario no quedó en la base de datos.");
            Assert.AreEqual(this.empleado.id, guardado.empleado);
            Assert.AreEqual(1500000.50m, guardado.valor);
            Assert.IsNull(guardado.fecha_fin);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Salarios!
                .Include(x => x._empleado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de salarios está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El salario insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._empleado, "No se cargó la relación con empleados.");
            Assert.AreEqual(this.empleado!.id, encontrado._empleado.id);
        }

        private void Actualizar()
        {
            this.entidad!.valor = 1750000.75m;
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Salarios!.First(x => x.id == this.entidad.id);
            Assert.AreEqual(1750000.75m, actualizado.valor, "No se actualizó el valor del salario.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Salarios, this.entidad!);

            var borrado = NuevaConexion().Salarios!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El salario no fue eliminado.");
        }
    }
}
