# Biblioteca

Sistema de gestión de biblioteca desarrollado en C# sobre .NET 10, utilizando
Entity Framework Core como ORM y SQL Server como motor de datos.

- **Materia:** Programación de Software
- **Institución:** ITM
- **Integrantes:** Santiago Carmona Zapata, Daniel Moncada Rodríguez, Juan José Gómez Jaramillo

## Estructura de la solución

```
biblioteca.sln
│
├── aplicaciones_libreria/          Biblioteca de clases
│   ├── Script.sql                  Creación de la base de datos y datos de ejemplo
│   ├── entidades/                  20 clases, una por tabla
│   ├── interfaces/IConexion.cs     Contrato de la conexión
│   └── implementaciones/           Conexion.cs (DbContext)
│
├── presentacion_consola/           Aplicación de consola
│
└── presentacion_pruebas/           Pruebas unitarias (MSTest)
```

## Requisitos

- Visual Studio 2022 con el SDK de .NET 10
- SQL Server (Express o Developer)
- Paquetes NuGet: `Microsoft.EntityFrameworkCore.SqlServer 10.0.12`, `MSTest 4.0.1`

## Ejecución

1. Ejecutar `aplicaciones_libreria/Script.sql` contra SQL Server. El script es
   re-ejecutable: elimina y vuelve a crear `biblioteca_db` con sus 20 tablas y datos.
2. Restaurar los paquetes NuGet sobre la solución.
3. Establecer `presentacion_consola` como proyecto de inicio y ejecutar.

La cadena de conexión se define en un único punto:

```
aplicaciones_libreria/nucleo/DatosGenerales.cs
```

```
server=localhost;database=biblioteca_db;Integrated Security=True;TrustServerCertificate=true;
```

Si el servidor se encuentra en otra instancia, reemplazar `localhost` por
`localhost\SQLEXPRESS` o `.\SQLEXPRESS`.

## Pruebas

Las 20 pruebas se ejecutan desde la raíz de la solución con:

```
dotnet test
```

Resultado esperado:

```
Correctas! - Con error: 0, Superado: 20, Omitido: 0, Total: 20
```

También desde el IDE: en Visual Studio, *Ver → Test Explorer → Run All*; en
Rider, *View → Tool Windows → Unit Tests → Run All*.

`presentacion_pruebas` contiene 20 clases de prueba, una por tabla. Cada clase expone un
único método `Ejecutar()` que recorre las cuatro operaciones —Insertar, Consultar,
Actualizar y Borrar— con aserciones en cada paso y validación de las relaciones
(`Include` / `ThenInclude`).

`PruebaBase.cs` centraliza la conexión y ofrece los métodos `Guardar`, `Modificar` y
`Eliminar`. Los registros padre los crea la propia prueba, por lo que no dependen de los
datos incluidos en `Script.sql` y las pruebas pueden ejecutarse de forma repetida. Un
`TestCleanup` elimina los registros creados al finalizar, incluso si la prueba falla.
