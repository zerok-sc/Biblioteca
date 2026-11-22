# Biblioteca

Sistema de gestión de biblioteca desarrollado en C# sobre .NET 10, con
Entity Framework Core, SQL Server, WebAPI y Razor Pages.

- **Materia:** Programación de Software
- **Institución:** ITM
- **Integrante:** Santiago Carmona Zapata

## Estructura de la solución

```
biblioteca.sln
│
├── aplicaciones_libreria/          Biblioteca de clases
│   ├── Script.sql                  Creación de la base de datos y datos de ejemplo
│   ├── entidades/                  24 clases, una por tabla
│   ├── configuraciones/            Mapeo Fluent API
│   ├── interfaces/                 IConexion e IRepository<T>
│   └── implementaciones/           Conexion.cs (DbContext) y Repository<T>
│
├── presentacion_consola/           Aplicación de consola
├── presentacion_pruebas/           Pruebas unitarias (MSTest)
├── servicios_biblioteca/           WebAPI CRUD, puerto 5130
├── presentacion_libreria/          Cliente HttpClient (IComunicaciones e IPresentacion<T>)
└── front_biblioteca/               Razor Pages, puerto 5131
```

## Requisitos

- SDK de .NET 10
- SQL Server 2022 (en este equipo hay un contenedor de Podman llamado `mssql`)
- Paquetes NuGet: `Microsoft.EntityFrameworkCore.SqlServer 10.0.12`,
  `Microsoft.EntityFrameworkCore.Design 10.0.12`, `Swashbuckle.AspNetCore 10.2.3`,
  `Newtonsoft.Json 13.0.3`, `MSTest 4.0.1`

## Puesta en marcha

1. Encender SQL Server:

   ```bash
   podman start mssql
   ```

   Si el contenedor no existe:

   ```bash
   podman run -d --name mssql -e ACCEPT_EULA=Y \
     -e MSSQL_SA_PASSWORD='Biblioteca#2026' \
     -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
   ```

2. Crear la base de datos con las 24 tablas y los datos de ejemplo. El script
   elimina y vuelve a crear `biblioteca_db`, así que borra cualquier cambio
   anterior (incluidas las fotos):

   ```bash
   podman cp aplicaciones_libreria/Script.sql mssql:/tmp/Script.sql
   podman exec mssql /opt/mssql-tools18/bin/sqlcmd -S localhost \
     -U sa -P 'Biblioteca#2026' -C -b -i /tmp/Script.sql
   ```

3. Terminal 1 — WebAPI:

   ```bash
   dotnet run --project servicios_biblioteca
   ```

   Swagger: `http://localhost:5130/swagger`

4. Terminal 2 — Razor Pages:

   ```bash
   dotnet run --project front_biblioteca
   ```

5. Abrir `http://localhost:5131` e ingresar con uno de estos usuarios:

| Usuario         | Clave            | Rol           | Acceso |
|-----------------|------------------|---------------|--------|
| `admin`         | `Biblioteca2026!` | ADMIN         | Todo el sistema |
| `bibliotecario` | `biblio123`      | BIBLIO        | Catálogo y operaciones; sin Usuarios, Roles, Auditorías ni Sesiones |
| `lector`        | `lector123`      | LECTOR        | Solo consultar el catálogo; sin botones de guardar/borrar |

## Cadena de conexión

Está en dos puntos, porque la WebAPI y las pruebas/consola son procesos distintos:

- `servicios_biblioteca/appsettings.json`
- `aplicaciones_libreria/nucleo/DatosGenerales.cs`

```
Server=localhost,1433;Database=biblioteca_db;User Id=sa;Password=Biblioteca#2026;TrustServerCertificate=True;
```

Para SQL Server en Windows con autenticación integrada, se puede cambiar por
`server=localhost\SQLEXPRESS;database=biblioteca_db;Integrated Security=True;TrustServerCertificate=true;`.
La contraseña mostrada es solo de desarrollo local.

## Autenticación y roles

La autenticación se configura una sola vez en `front_biblioteca/Program.cs`:
cookies + `Claims` (`ClaimTypes.Role`), con `UseAuthentication()` y
`UseAuthorization()`. No hay usuario global, estático ni Singleton.

- `Pages/Login.cshtml.cs` valida la clave contra la tabla `Usuarios` a través de
  la WebAPI (`Usuarios/Login`), que compara el `SHA256` de `clave + sal`.
- Cada página de `Pages/Ventanas` lleva su `[Authorize]` con el rol permitido.
- El rol LECTOR además se bloquea en los handlers de `PaginaBase<T>`, no solo en
  los botones: aunque llame al handler, no puede insertar, modificar ni borrar.
- `Pages/Index.cshtml.cs` filtra el menú con `PuedeVer()` para que un usuario
  cualquiera no vea enlaces a los que no tiene acceso.
- El botón **Crear usuario** aparece solo para ADMIN, en la ventana Usuarios y en
  el menú. `Pages/CrearUsuario.cshtml.cs` envía el alta a `Usuarios/CrearUsuario`:
  la WebAPI genera la sal, calcula el `SHA256` de `clave + sal`, valida duplicados
  y toma los roles y personas desde la base de datos. La clave nunca se guarda
  en texto plano.

## Errores y mensajes

Se usa el mismo patrón de los proyectos del curso (`PiscinaSC` / `horarios`):

- `front_biblioteca/nucleo/LogConversor.cs` guarda el mensaje en `ViewData["Mensaje"]`.
- `Pages/Emergentes/Mensajes.cshtml` muestra un popup con botón **Cerrar**.
- Los mensajes de éxito usan el mismo popup.

Así se ve el error de la WebAPI en pantalla en vez de ocultarlo con un `catch` vacío.

## Imágenes de perfil

`Pages/Perfil.cshtml` permite subir una imagen (máximo 2 MB) con `IFormFile`.
La WebAPI la guarda como `VARBINARY(MAX)` en `Usuarios.foto`, junto con
`Usuarios.foto_tipo`, y la página la muestra con `data:<tipo>;base64,...`.
También aparece en la tabla de Usuarios y en el `Login` de la API.

## Préstamos

En `Pages/Ventanas/Prestamos.cshtml`, los roles ADMIN y BIBLIO pueden registrar un
préstamo:

1. Buscan al usuario por nombre o cédula.
2. Pulsan **Seleccionar**.
3. Indican tiempo en días, fecha de inicio, fecha final, estado y empleado.
4. Pulsan **Registrar préstamo**.

El código se genera automáticamente. Si la fecha final se deja vacía, se calcula
con la fecha de inicio y el tiempo. La tabla `Prestamos` guarda `tiempo`,
`fecha_inicio`, `fecha_fin` y `estado` (llave foránea a `Estados`), además de la
persona y el empleado. El rol LECTOR no puede entrar a esta página.

## Nombres en lugar de ids

`front_biblioteca/nucleo/Catalogos.cs` carga los catálogos que usan las ventanas.
Las tablas muestran `NombreEditorial`, `NombreIdioma`, `NombrePersona`,
`NombreEstado`, etc., y los formularios usan listas desplegables con
`OpcionesEditoriales`, `OpcionesIdiomas`, `OpcionesPersonas`, etc.

Los ids siguen existiendo en la base de datos y en los campos ocultos, pero ya no
se muestran como columnas. Así un ADMIN o BIBLIO elige **Alfaguara** o **Español**
en lugar de tener que recordar que `3` es una editorial o `1` un idioma.

## Migraciones

```
dotnet tool install --global dotnet-ef --version 10.0.12
dotnet ef migrations add InitialBiblioteca --project aplicaciones_libreria --startup-project presentacion_consola
dotnet ef database update --project aplicaciones_libreria --startup-project presentacion_consola
```

`Script.sql` y las migraciones producen el mismo esquema de 24 tablas. Se usa uno
u otro, no ambos sobre la misma base de datos.

## Pruebas

```
dotnet test
```

Resultado esperado: `Correctas! - Con error: 0, Superado: 20, Omitido: 0, Total: 20`.

`PruebaBase.cs` valida filas afectadas y usa `Assert.Fail` en la limpieza, para
evitar falsos positivos y errores silenciados.
