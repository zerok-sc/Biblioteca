using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class PrestamosPruebas : PruebaBase
    {
        private Personas? persona;
        private Empleados? empleado;
        private Prestamos? entidad;

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
            this.persona = CrearPersona();
            this.empleado = CrearEmpleado();

            this.entidad = Guardar(c => c.Prestamos, new Prestamos
            {
                codigo = Unico("PRES"),
                persona = this.persona.id,
                empleado = this.empleado.id,
                fecha = new DateTime(2025, 3, 10, 9, 0, 0)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id.");

            var guardado = NuevaConexion().Prestamos!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El registro no quedó en la base de datos.");
            Assert.AreEqual(this.entidad.codigo, guardado.codigo);
            Assert.AreEqual(this.persona.id, guardado.persona);
            Assert.AreEqual(this.empleado.id, guardado.empleado);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Prestamos!
                .Include(x => x._persona)
                .Include(x => x._empleado)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El registro insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._persona, "No se cargó la relación _persona.");
            Assert.AreEqual(this.persona!.id, encontrado._persona.id);
            Assert.IsNotNull(encontrado._empleado, "No se cargó la relación _empleado.");
            Assert.AreEqual(this.empleado!.id, encontrado._empleado.id);
        }

        private void Actualizar()
        {
            this.entidad!.fecha = new DateTime(2025, 4, 1, 14, 30, 0);
            Modificar(this.entidad!);

            var actualizado = NuevaConexion().Prestamos!.First(x => x.id == this.entidad!.id);
            Assert.AreEqual(new DateTime(2025, 4, 1, 14, 30, 0), actualizado.fecha, "No se actualizó la fecha.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Prestamos, this.entidad!);

            var borrado = NuevaConexion().Prestamos!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El registro no fue eliminado.");
        }
    }
}
