using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class ComprasPruebas : PruebaBase
    {
        private Proveedores? proveedor;
        private Empleados? empleado;
        private Compras? entidad;

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
            this.proveedor = CrearProveedor();
            this.empleado = CrearEmpleado();

            this.entidad = Guardar(c => c.Compras, new Compras
            {
                fecha = new DateTime(2025, 5, 5, 11, 0, 0),
                total = 450000.00m,
                proveedor = this.proveedor.id,
                empleado = this.empleado.id
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Compras!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(450000.00m, guardado.total);
            Assert.AreEqual(this.proveedor.id, guardado.proveedor);
            Assert.AreEqual(this.empleado.id, guardado.empleado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Compras!
                .Include(x => x._proveedor)
                .Include(x => x._empleado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._proveedor, "No se cargó la relación _proveedor.");
            Assert.AreEqual(this.proveedor!.id, encontrado._proveedor.id);
            Assert.IsNotNull(encontrado._empleado, "No se cargó la relación _empleado.");
            Assert.AreEqual(this.empleado!.id, encontrado._empleado.id);
        }

        private void Actualizar()
        {
            this.entidad!.total = 900000.25m;
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Compras!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(900000.25m, actualizado.total, "No se actualizó el total.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Compras, this.entidad!);

            var borrado = NuevaConexion().Compras!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
