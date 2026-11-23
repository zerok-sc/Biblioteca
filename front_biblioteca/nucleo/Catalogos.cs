using aplicaciones_libreria.entidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using presentacion_libreria.interfaces;

namespace front_biblioteca.nucleo
{
    public class Catalogos
    {
        private readonly IPresentacion<Personas> personas;
        private readonly IPresentacion<Empleados> empleados;
        private readonly IPresentacion<Estados> estados;
        private readonly IPresentacion<Editoriales> editoriales;
        private readonly IPresentacion<Idiomas> idiomas;
        private readonly IPresentacion<Libros> libros;
        private readonly IPresentacion<Autores> autores;
        private readonly IPresentacion<Categorias> categorias;
        private readonly IPresentacion<Secciones> secciones;
        private readonly IPresentacion<Estanterias> estanterias;
        private readonly IPresentacion<Ubicaciones> ubicaciones;
        private readonly IPresentacion<Ejemplares> ejemplares;
        private readonly IPresentacion<Proveedores> proveedores;
        private readonly IPresentacion<Roles> roles;
        private readonly IPresentacion<Usuarios> usuarios;
        private readonly IPresentacion<Prestamos> prestamos;
        private readonly IPresentacion<Prestamos_Libros> prestamosLibros;
        private readonly IPresentacion<Multas> multas;

        public List<Personas> ListaPersonas { get; private set; } = new();
        public List<Empleados> ListaEmpleados { get; private set; } = new();
        public List<Estados> ListaEstados { get; private set; } = new();
        public List<Editoriales> ListaEditoriales { get; private set; } = new();
        public List<Idiomas> ListaIdiomas { get; private set; } = new();
        public List<Libros> ListaLibros { get; private set; } = new();
        public List<Autores> ListaAutores { get; private set; } = new();
        public List<Categorias> ListaCategorias { get; private set; } = new();
        public List<Secciones> ListaSecciones { get; private set; } = new();
        public List<Estanterias> ListaEstanterias { get; private set; } = new();
        public List<Ubicaciones> ListaUbicaciones { get; private set; } = new();
        public List<Ejemplares> ListaEjemplares { get; private set; } = new();
        public List<Proveedores> ListaProveedores { get; private set; } = new();
        public List<Roles> ListaRoles { get; private set; } = new();
        public List<Usuarios> ListaUsuarios { get; private set; } = new();
        public List<Prestamos> ListaPrestamos { get; private set; } = new();
        public List<Prestamos_Libros> ListaPrestamosLibros { get; private set; } = new();
        public List<Multas> ListaMultas { get; private set; } = new();

        private bool cargado;

        public Catalogos(
            IPresentacion<Personas> personas,
            IPresentacion<Empleados> empleados,
            IPresentacion<Estados> estados,
            IPresentacion<Editoriales> editoriales,
            IPresentacion<Idiomas> idiomas,
            IPresentacion<Libros> libros,
            IPresentacion<Autores> autores,
            IPresentacion<Categorias> categorias,
            IPresentacion<Secciones> secciones,
            IPresentacion<Estanterias> estanterias,
            IPresentacion<Ubicaciones> ubicaciones,
            IPresentacion<Ejemplares> ejemplares,
            IPresentacion<Proveedores> proveedores,
            IPresentacion<Roles> roles,
            IPresentacion<Usuarios> usuarios,
            IPresentacion<Prestamos> prestamos,
            IPresentacion<Prestamos_Libros> prestamosLibros,
            IPresentacion<Multas> multas)
        {
            this.personas = personas;
            this.empleados = empleados;
            this.estados = estados;
            this.editoriales = editoriales;
            this.idiomas = idiomas;
            this.libros = libros;
            this.autores = autores;
            this.categorias = categorias;
            this.secciones = secciones;
            this.estanterias = estanterias;
            this.ubicaciones = ubicaciones;
            this.ejemplares = ejemplares;
            this.proveedores = proveedores;
            this.roles = roles;
            this.usuarios = usuarios;
            this.prestamos = prestamos;
            this.prestamosLibros = prestamosLibros;
            this.multas = multas;
        }

        public async Task CargarAsync()
        {
            if (cargado)
                return;

            var tareas = new List<Task>
            {
                CargarPersonas(),
                CargarEmpleados(),
                CargarEstados(),
                CargarEditoriales(),
                CargarIdiomas(),
                CargarLibros(),
                CargarAutores(),
                CargarCategorias(),
                CargarSecciones(),
                CargarEstanterias(),
                CargarUbicaciones(),
                CargarEjemplares(),
                CargarProveedores(),
                CargarRoles(),
                CargarUsuarios(),
                CargarPrestamos(),
                CargarPrestamosLibros(),
                CargarMultas()
            };

            await Task.WhenAll(tareas);
            cargado = true;
        }

        public async Task RecargarAsync()
        {
            cargado = false;
            await CargarAsync();
        }

        private async Task CargarPersonas() => ListaPersonas = await personas.Consultar();
        private async Task CargarEmpleados() => ListaEmpleados = await empleados.Consultar();
        private async Task CargarEstados() => ListaEstados = await estados.Consultar();
        private async Task CargarEditoriales() => ListaEditoriales = await editoriales.Consultar();
        private async Task CargarIdiomas() => ListaIdiomas = await idiomas.Consultar();
        private async Task CargarLibros() => ListaLibros = await libros.Consultar();
        private async Task CargarAutores() => ListaAutores = await autores.Consultar();
        private async Task CargarCategorias() => ListaCategorias = await categorias.Consultar();
        private async Task CargarSecciones() => ListaSecciones = await secciones.Consultar();
        private async Task CargarEstanterias() => ListaEstanterias = await estanterias.Consultar();
        private async Task CargarUbicaciones() => ListaUbicaciones = await ubicaciones.Consultar();
        private async Task CargarEjemplares() => ListaEjemplares = await ejemplares.Consultar();
        private async Task CargarProveedores() => ListaProveedores = await proveedores.Consultar();
        private async Task CargarRoles() => ListaRoles = await roles.Consultar();
        private async Task CargarUsuarios() => ListaUsuarios = await usuarios.Consultar();
        private async Task CargarPrestamos() => ListaPrestamos = await prestamos.Consultar();
        private async Task CargarPrestamosLibros() => ListaPrestamosLibros = await prestamosLibros.Consultar();
        private async Task CargarMultas() => ListaMultas = await multas.Consultar();

        public string NombrePersona(int id) =>
            id == 0 ? "No asignado" : ListaPersonas.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreEmpleado(int id)
        {
            if (id == 0)
                return "No asignado";

            var empleado = ListaEmpleados.FirstOrDefault(x => x.id == id);
            return empleado is null
                ? "No asignado"
                : $"{NombrePersona(empleado.persona)} ({empleado.cargo})";
        }

        public string NombreEstado(int id) =>
            id == 0 ? "No asignado" : ListaEstados.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string CodigoEstado(int id) =>
            ListaEstados.FirstOrDefault(x => x.id == id)?.codigo ?? "";

        public int IdEstado(string codigo) =>
            ListaEstados.FirstOrDefault(x => x.codigo == codigo)?.id ?? 0;

        public bool EstadoEsActivo(int id) =>
            CodigoEstado(id) is "PRE" or "ATR";

        public bool PrestamoActivo(Prestamos prestamo) =>
            EstadoEsActivo(prestamo.estado);

        public bool PrestamoAtrasado(Prestamos prestamo) =>
            PrestamoActivo(prestamo) && prestamo.fecha_fin.Date < DateTime.Today;

        public int DiasAtraso(Prestamos prestamo) =>
            Math.Max(0, (DateTime.Today - prestamo.fecha_fin.Date).Days);

        public int DiasRestantes(Prestamos prestamo) =>
            (prestamo.fecha_fin.Date - DateTime.Today).Days;

        public string NombreEditorial(int id) =>
            id == 0 ? "No asignado" : ListaEditoriales.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreIdioma(int id) =>
            id == 0 ? "No asignado" : ListaIdiomas.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreLibro(int id)
        {
            if (id == 0)
                return "No asignado";

            var libro = ListaLibros.FirstOrDefault(x => x.id == id);
            return libro is null ? "No asignado" : $"{libro.codigo} - {libro.nombre}";
        }

        public string NombreAutor(int id)
        {
            if (id == 0)
                return "No asignado";

            var autor = ListaAutores.FirstOrDefault(x => x.id == id);
            return autor is null ? "No asignado" : $"{autor.nombre} {autor.apellido}".Trim();
        }

        public string NombreCategoria(int id) =>
            id == 0 ? "No asignado" : ListaCategorias.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreSeccion(int id) =>
            id == 0 ? "No asignado" : ListaSecciones.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreEstanteria(int id)
        {
            var estanteria = ListaEstanterias.FirstOrDefault(x => x.id == id);
            if (estanteria is null)
                return "No asignado";

            var seccion = estanteria.seccion == 0
                ? null
                : ListaSecciones.FirstOrDefault(x => x.id == estanteria.seccion);

            return seccion is null
                ? $"Estantería {estanteria.numero}"
                : $"Estantería {estanteria.numero} ({seccion.nombre})";
        }

        public string NombreUbicacion(int id)
        {
            var ubicacion = ListaUbicaciones.FirstOrDefault(x => x.id == id);
            if (ubicacion is null)
                return "No asignado";

            var partes = new List<string>();
            if (!string.IsNullOrWhiteSpace(ubicacion.descripcion))
                partes.Add(ubicacion.descripcion.Trim());
            partes.Add($"Piso {ubicacion.piso}");
            partes.Add(NombreEstanteria(ubicacion.estanteria));

            return string.Join(" · ", partes);
        }

        public string NombreEjemplar(int id) =>
            id == 0 ? "No asignado" : ListaEjemplares.FirstOrDefault(x => x.id == id)?.codigo ?? "No asignado";

        public string NombreProveedor(int id) =>
            id == 0 ? "No asignado" : ListaProveedores.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreRol(int id) =>
            id == 0 ? "No asignado" : ListaRoles.FirstOrDefault(x => x.id == id)?.nombre ?? "No asignado";

        public string NombreUsuario(int id) =>
            id == 0 ? "No asignado" : ListaUsuarios.FirstOrDefault(x => x.id == id)?.nombre_usuario ?? "No asignado";

        public string NombrePrestamo(int id) =>
            id == 0 ? "No asignado" : ListaPrestamos.FirstOrDefault(x => x.id == id)?.codigo ?? "No asignado";

        public string NombrePrestamoLibro(int id)
        {
            if (id == 0)
                return "No asignado";

            var prestamoLibro = ListaPrestamosLibros.FirstOrDefault(x => x.id == id);
            return prestamoLibro is null
                ? "No asignado"
                : $"{NombrePrestamo(prestamoLibro.prestamo)} / {NombreEjemplar(prestamoLibro.ejemplar)}";
        }

        public string NombreLibroDeEjemplar(int ejemplarId)
        {
            var ejemplar = ListaEjemplares.FirstOrDefault(x => x.id == ejemplarId);
            return ejemplar is null ? "No asignado" : NombreLibro(ejemplar.libro);
        }

        public List<Prestamos_Libros> PrestamosLibrosDe(int prestamoId) =>
            ListaPrestamosLibros
                .Where(x => x.prestamo == prestamoId)
                .OrderBy(x => x.id)
                .ToList();

        public string EjemplaresDePrestamo(int prestamoId)
        {
            var ejemplares = PrestamosLibrosDe(prestamoId)
                .Select(x => NombreEjemplar(x.ejemplar))
                .ToList();

            return ejemplares.Count == 0 ? "Sin ejemplares" : string.Join(", ", ejemplares);
        }

        public string UbicacionesDePrestamo(int prestamoId)
        {
            var ubicaciones = PrestamosLibrosDe(prestamoId)
                .Select(x => ListaEjemplares.FirstOrDefault(e => e.id == x.ejemplar)?.ubicacion ?? 0)
                .Where(x => x > 0)
                .Distinct()
                .Select(NombreUbicacion)
                .ToList();

            return ubicaciones.Count == 0 ? "Sin ubicación" : string.Join(", ", ubicaciones);
        }

        public int CantidadEjemplaresDePrestamo(int prestamoId) =>
            PrestamosLibrosDe(prestamoId).Count;

        public string NombrePersonaDePrestamoLibro(int prestamoLibroId)
        {
            var prestamoLibro = ListaPrestamosLibros.FirstOrDefault(x => x.id == prestamoLibroId);
            if (prestamoLibro is null)
                return "No asignado";

            var prestamo = ListaPrestamos.FirstOrDefault(x => x.id == prestamoLibro.prestamo);
            return prestamo is null ? "No asignado" : NombrePersona(prestamo.persona);
        }

        public string NombreLibroDePrestamoLibro(int prestamoLibroId)
        {
            var prestamoLibro = ListaPrestamosLibros.FirstOrDefault(x => x.id == prestamoLibroId);
            return prestamoLibro is null ? "No asignado" : NombreLibroDeEjemplar(prestamoLibro.ejemplar);
        }

        public int DiasAtrasoDePrestamoLibro(int prestamoLibroId)
        {
            var prestamoLibro = ListaPrestamosLibros.FirstOrDefault(x => x.id == prestamoLibroId);
            if (prestamoLibro is null)
                return 0;

            var prestamo = ListaPrestamos.FirstOrDefault(x => x.id == prestamoLibro.prestamo);
            return prestamo is null ? 0 : DiasAtraso(prestamo);
        }

        // Días cobrados cuando se generó la multa. No se recalculan con hoy:
        // si se usa DateTime.Today, el valor sigue creciendo después de pagar.
        public int DiasAtrasoDeMulta(Multas multa)
        {
            if (multa.fecha == default)
                return 0;

            var detalle = ListaPrestamosLibros.FirstOrDefault(x => x.id == multa.prestamo_libro);
            if (detalle is null)
                return 0;

            var prestamo = ListaPrestamos.FirstOrDefault(x => x.id == detalle.prestamo);
            if (prestamo is null)
                return 0;

            return Math.Max(0, (multa.fecha.Date - prestamo.fecha_fin.Date).Days);
        }

        public List<SelectListItem> OpcionesPersonas =>
            ConVacio(ListaPersonas.Select(x => new SelectListItem(
                $"{x.nombre} ({x.cedula})", x.id.ToString())));

        public List<SelectListItem> OpcionesEmpleados =>
            ConVacio(ListaEmpleados.Select(x => new SelectListItem(NombreEmpleado(x.id), x.id.ToString())));

        public List<SelectListItem> OpcionesEstados =>
            ConVacio(ListaEstados
                .Where(x => x.codigo != "PAG")
                .Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesEstadosMulta =>
            ConVacio(ListaEstados
                .Where(x => x.codigo is "ACT" or "INA" or "PAG")
                .Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesEditoriales =>
            ConVacio(ListaEditoriales.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesIdiomas =>
            ConVacio(ListaIdiomas.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesLibros =>
            ConVacio(ListaLibros.Select(x => new SelectListItem(NombreLibro(x.id), x.id.ToString())));

        public List<SelectListItem> OpcionesAutores =>
            ConVacio(ListaAutores.Select(x => new SelectListItem(NombreAutor(x.id), x.id.ToString())));

        public List<SelectListItem> OpcionesCategorias =>
            ConVacio(ListaCategorias.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesSecciones =>
            ConVacio(ListaSecciones.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesEstanterias =>
            ConVacio(ListaEstanterias.Select(x => new SelectListItem(NombreEstanteria(x.id), x.id.ToString())));

        public List<SelectListItem> OpcionesUbicaciones =>
            ConVacio(ListaUbicaciones.Select(x => new SelectListItem(NombreUbicacion(x.id), x.id.ToString())));

        public List<SelectListItem> OpcionesEjemplares =>
            ConVacio(ListaEjemplares.Select(x => new SelectListItem(x.codigo, x.id.ToString())));

        public List<SelectListItem> OpcionesProveedores =>
            ConVacio(ListaProveedores.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesRoles =>
            ConVacio(ListaRoles.Select(x => new SelectListItem(x.nombre, x.id.ToString())));

        public List<SelectListItem> OpcionesUsuarios =>
            ConVacio(ListaUsuarios.Select(x => new SelectListItem(x.nombre_usuario, x.id.ToString())));

        public List<SelectListItem> OpcionesPrestamos =>
            ConVacio(ListaPrestamos.Select(x => new SelectListItem(x.codigo, x.id.ToString())));

        public List<SelectListItem> OpcionesPrestamosLibros =>
            ConVacio(ListaPrestamosLibros.Select(x => new SelectListItem(NombrePrestamoLibro(x.id), x.id.ToString())));

        private static List<SelectListItem> ConVacio(IEnumerable<SelectListItem> items)
        {
            var lista = new List<SelectListItem>
            {
                new() { Value = "", Text = "Seleccione..." }
            };

            lista.AddRange(items);
            return lista;
        }
    }
}
