# Biblioteca

WebAPI + Razor Pages + EF Core / SQL Server (.NET 10).

## Correr el proyecto

```bash
# 1. SQL Server (si el contenedor no existe, usa podman run del paso 4)
podman start mssql

# 2. Base de datos (borra y recrea biblioteca_db)
podman cp "$PWD/aplicaciones_libreria/Script.sql" mssql:/tmp/Script_actual.sql
podman exec mssql /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Biblioteca#2026' -C -b \
  -i /tmp/Script_actual.sql
```

Terminal 1:

```bash
dotnet run --project servicios_biblioteca
```

Terminal 2:

```bash
dotnet run --project front_biblioteca
```

- Front: http://localhost:5131
- Swagger: http://localhost:5130/swagger

Si el contenedor no existe:

```bash
podman run -d --name mssql -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='Biblioteca#2026' \
  -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
```

## Usuarios

| Usuario | Clave | Rol |
|---|---|---|
| `admin` | `Biblioteca2026!` | ADMIN |
| `bibliotecario` | `biblio123` | BIBLIO |
| `lector` | `lector123` | LECTOR |

ADMIN ve todo. BIBLIO gestiona catálogo y préstamos. LECTOR solo consulta y ve **Mis préstamos**.

## Cadena de conexión

```
Server=localhost,1433;Database=biblioteca_db;User Id=sa;Password=Biblioteca#2026;TrustServerCertificate=True;
```

Está en `servicios_biblioteca/appsettings.json` y `aplicaciones_libreria/nucleo/DatosGenerales.cs`.

## Cosas útiles

- **Catálogos**: alta, edición y borrado; búsqueda por texto; llaves foráneas se eligen por nombre.
- **Préstamos**: registrar varias copias, devolver, renovar, comprobante imprimible y exportar CSV.
- **Multas**: 2.000 por día de atraso, se generan al devolver; botón **Pagar**.
- **Compras**: elige libro, cantidad y costo unitario; el total se calcula solo y crea los ejemplares en `DIS`. Una compra con libros no se puede borrar.
- **Auditorías**: solo ADMIN y solo lectura. Registra altas, ediciones, borrados, `PRESTAR`, `DEVOLVER`, `RENOVAR`, `MULTA`, `PAGAR` y `COMPRA`, con usuario, IP y valores anteriores/nuevos.
- **Sesiones**: ADMIN ve ingresos, intentos fallidos y cierres con IP, agente y motivo.
- **Borrados**: si el registro tiene datos relacionados, devuelve un mensaje claro en vez de error 500.
- **Crear usuario**: exige elegir rol y persona; la persona se muestra con cédula para evitar confusiones.
- **Fotos de perfil**: máximo 2 MB, se guardan en `Usuarios.foto`.

Si vienes de una versión anterior, aplica `aplicaciones_libreria/ActualizarCompras.sql`
para agregar la tabla `Compras_Libros` sin borrar la base.

## Pruebas

```bash
dotnet test biblioteca.sln
```

Esperado: 20 superadas, 0 errores.
