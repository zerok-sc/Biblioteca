using aplicaciones_libreria.entidades;
using Microsoft.EntityFrameworkCore;

namespace presentacion_pruebas
{
    [TestClass]
    public class EmpleadosPruebas : PruebaBase
    {
        private Personas? persona;
        private Empleados? entidad;

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

            this.entidad = Guardar(c => c.Empleados, new Empleados
            {
                persona = this.persona.id,
                cargo = "Bibliotecario",
                fecha_contratacion = new DateTime(2024, 2, 1)
            });

            Assert.IsTrue(this.entidad.id > 0, "No se generó el id del empleado.");

            var guardado = NuevaConexion().Empleados!.FirstOrDefault(x => x.id == this.entidad.id);
            Assert.IsNotNull(guardado, "El empleado no quedó en la base de datos.");
            Assert.AreEqual(this.persona.id, guardado.persona);
            Assert.AreEqual("Bibliotecario", guardado.cargo);
            Assert.AreEqual(new DateTime(2024, 2, 1), guardado.fecha_contratacion);
        }

        private void Consultar()
        {
            var lista = NuevaConexion().Empleados!
                .Include(x => x._persona)
                .ToList();
            Assert.IsTrue(lista.Count > 0, "La lista de empleados está vacía.");

            var encontrado = lista.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNotNull(encontrado, "El empleado insertado no aparece en la consulta.");
            Assert.IsNotNull(encontrado._persona, "No se cargó la relación con personas.");
            Assert.AreEqual(this.persona!.id, encontrado._persona.id);
        }

        private void Actualizar()
        {
            this.entidad!.cargo = "Gerente";
            Modificar(this.entidad);

            var actualizado = NuevaConexion().Empleados!.First(x => x.id == this.entidad.id);
            Assert.AreEqual("Gerente", actualizado.cargo, "No se actualizó el cargo.");
        }

        private void Borrar()
        {
            Eliminar(c => c.Empleados, this.entidad!);

            var borrado = NuevaConexion().Empleados!.FirstOrDefault(x => x.id == this.entidad!.id);
            Assert.IsNull(borrado, "El empleado no fue eliminado.");
        }
    }
}
