using aplicaciones_libreria.implementaciones;
using aplicaciones_libreria.interfaces;
using aplicaciones_libreria.nucleo;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = System.Text.Encoding.UTF8;

try
{
    IConexion conexion = new Conexion();
    conexion.StringConexion = DatosGenerales.StringConexion();


    var lista_personas = conexion.Personas!.ToList();

    Console.WriteLine("---------- PERSONAS ----------");
    foreach (var persona in lista_personas)
        Console.WriteLine($"{persona.id} | {persona.cedula} | {persona.nombre} | {persona.telefono}");

    var lista_libros = conexion.Libros!
        .Include(x => x._editorial)
        .Include(x => x._idioma)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("---------- LIBROS ----------");
    foreach (var libro in lista_libros)
        Console.WriteLine($"{libro.codigo} | {libro.nombre} | {libro._editorial?.nombre} | {libro._idioma?.nombre}");

    var lista_prestamos = conexion.Prestamos_Libros!
        .Include(x => x._prestamo)
            .ThenInclude(x => x!._persona)
        .Include(x => x._prestamo)
            .ThenInclude(x => x!._empleado)
                .ThenInclude(x => x!._persona)
        .Include(x => x._ejemplar)
            .ThenInclude(x => x!._libro)
        .Include(x => x._estado)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("---------- PRÉSTAMOS ----------");
    foreach (var detalle in lista_prestamos)
        Console.WriteLine($"{detalle._prestamo?.codigo} | {detalle._prestamo?._persona?.nombre} | " +
                          $"{detalle._ejemplar?._libro?.nombre} | Atendió: {detalle._prestamo?._empleado?._persona?.nombre} | " +
                          $"Estado: {detalle._estado?.nombre}");

    var lista_empleados = conexion.Empleados!
        .Include(x => x._persona)
        .Include(x => x.salarios)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("---------- EMPLEADOS ----------");
    foreach (var empleado in lista_empleados)
        Console.WriteLine($"{empleado.id} | {empleado._persona?.nombre} | {empleado.cargo} | " +
                          $"Salario: {empleado.salarios?.FirstOrDefault()?.valor:N2}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

Console.WriteLine();
Console.WriteLine("presentacion_consola");
