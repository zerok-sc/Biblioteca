using aplicaciones_libreria.entidades;
using front_biblioteca.nucleo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using presentacion_libreria.interfaces;

namespace front_biblioteca.Pages.Ventanas
{
    [Authorize(Roles = "ADMIN,BIBLIO")]
    public class ComprasModel : PaginaBase<Compras>
    {
        private readonly IPresentacion<Compras_Libros> detalles;
        private readonly IPresentacion<Ejemplares> ejemplares;

        [BindProperty] public int LibroSeleccionado { get; set; }
        [BindProperty] public int Cantidad { get; set; } = 1;
        [BindProperty] public decimal CostoUnitario { get; set; }
        [BindProperty] public int UbicacionSeleccionada { get; set; }

        public List<Compras_Libros> Detalles { get; private set; } = new();

        public ComprasModel(
            IPresentacion<Compras> presentacion,
            Catalogos catalogos,
            IPresentacion<Compras_Libros> detalles,
            IPresentacion<Ejemplares> ejemplares)
            : base(presentacion, catalogos)
        {
            this.detalles = detalles;
            this.ejemplares = ejemplares;
        }

        public override async Task OnGetAsync()
        {
            await base.OnGetAsync();
            await CargarDetallesAsync();
        }

        public List<Compras_Libros> DetallesDe(int compraId) =>
            Detalles.Where(x => x.compra == compraId).ToList();

        public decimal Subtotal(Compras_Libros detalle) =>
            detalle.cantidad * detalle.costo_unitario;

        public async Task<IActionResult> OnPostGuardarCompraAsync()
        {
            if (User.IsInRole("LECTOR"))
            {
                Avisar("El rol Lector solo puede consultar.");
                await RecargarAsync();
                return Page();
            }

            if (Actual.id > 0)
                return await ActualizarCompraAsync();

            return await CrearCompraAsync();
        }

        private async Task<IActionResult> CrearCompraAsync()
        {
            await Catalogos.CargarAsync();

            if (!Catalogos.ListaLibros.Any(x => x.id == LibroSeleccionado))
            {
                Avisar("Seleccione un libro válido.");
                await RecargarAsync();
                return Page();
            }

            if (Cantidad <= 0)
            {
                Avisar("La cantidad debe ser mayor que cero.");
                await RecargarAsync();
                return Page();
            }

            if (CostoUnitario <= 0)
            {
                Avisar("El costo unitario debe ser mayor que cero.");
                await RecargarAsync();
                return Page();
            }

            if (!Catalogos.ListaUbicaciones.Any(x => x.id == UbicacionSeleccionada))
            {
                Avisar("Seleccione una ubicación válida.");
                await RecargarAsync();
                return Page();
            }

            var estadoDisponible = Catalogos.IdEstado("DIS");
            if (estadoDisponible == 0)
            {
                Avisar("No existe el estado Disponible en la base de datos.");
                await RecargarAsync();
                return Page();
            }

            var total = Cantidad * CostoUnitario;

            try
            {
                var compra = await presentacion.Insertar(new Compras
                {
                    fecha = Actual.fecha == default ? DateTime.Today : Actual.fecha,
                    total = total,
                    proveedor = Actual.proveedor,
                    empleado = Actual.empleado
                });

                var detalle = await detalles.Insertar(new Compras_Libros
                {
                    compra = compra.id,
                    libro = LibroSeleccionado,
                    cantidad = Cantidad,
                    costo_unitario = CostoUnitario
                });

                var prefijo = $"EJM-{DateTime.Now:yyyyMMddHHmmssfff}";
                for (var i = 1; i <= Cantidad; i++)
                {
                    await ejemplares.Insertar(new Ejemplares
                    {
                        codigo = $"{prefijo}-{i}",
                        estado = estadoDisponible,
                        libro = LibroSeleccionado,
                        ubicacion = UbicacionSeleccionada
                    });
                }

                await RegistrarAuditoriaAsync("ALTA", null, compra, compra.id);
                await RegistrarAuditoriaAsync("COMPRA", null, detalle, detalle.id, "Compras_Libros");

                Avisar($"Compra registrada por {total:N0}. Se crearon {Cantidad} ejemplar(es) disponibles.");
                MostrarFormulario = false;
                LibroSeleccionado = 0;
                Cantidad = 1;
                CostoUnitario = 0;
                UbicacionSeleccionada = 0;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        private async Task<IActionResult> ActualizarCompraAsync()
        {
            try
            {
                var guardada = (await presentacion.Consultar()).FirstOrDefault(x => x.id == Actual.id);
                if (guardada is null)
                {
                    Avisar("La compra ya no existe.");
                    MostrarFormulario = false;
                    await RecargarAsync();
                    return Page();
                }

                var anteriores = Snapshot(guardada);
                Actual.total = guardada.total;
                await presentacion.Actualizar(Actual);
                await RegistrarAuditoriaAsync("EDICION", anteriores, Actual, Actual.id);

                Avisar("Compra actualizada correctamente.");
                MostrarFormulario = false;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostBorrarCompraAsync(int id)
        {
            if (User.IsInRole("LECTOR"))
            {
                Avisar("El rol Lector solo puede consultar.");
                await RecargarAsync();
                return Page();
            }

            try
            {
                var detalle = (await detalles.Consultar()).FirstOrDefault(x => x.compra == id);
                if (detalle is not null)
                {
                    Avisar("No se puede borrar: la compra tiene libros registrados.");
                    await RecargarAsync();
                    return Page();
                }

                var compra = (await presentacion.Consultar()).FirstOrDefault(x => x.id == id);
                await presentacion.Borrar(new Compras { id = id });
                await RegistrarAuditoriaAsync("BAJA", Snapshot(compra), null, id);
                Avisar("Compra eliminada correctamente.");
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData);
            }

            await RecargarAsync();
            return Page();
        }

        private async Task RecargarAsync()
        {
            await Catalogos.RecargarAsync();
            await CargarAsync();
            await CargarDetallesAsync();
        }

        private async Task CargarDetallesAsync()
        {
            try
            {
                Detalles = await detalles.Consultar();
            }
            catch
            {
                Detalles = new List<Compras_Libros>();
            }
        }
    }
}
