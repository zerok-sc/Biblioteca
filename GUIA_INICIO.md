# Guía rápida de inicio y prueba

Pasos mínimos para dejar el sistema corriendo y probar el alta de datos.

## 1. Base de datos

```bash
podman start mssql
podman cp aplicaciones_libreria/Script.sql mssql:/tmp/Script.sql
podman exec mssql /opt/mssql-tools18/bin/sqlcmd -S localhost \
  -U sa -P 'Biblioteca#2026' -C -b -i /tmp/Script.sql
```

El script crea `biblioteca_db` con 25 tablas y datos de ejemplo. Borra lo que
había antes, incluidas las fotos.

## 2. Servicios

Terminal 1:

```bash
dotnet run --project servicios_biblioteca
```

Terminal 2:

```bash
dotnet run --project front_biblioteca
```

URLs:

- Razor Pages: `http://localhost:5131`
- WebAPI: `http://localhost:5130`
- Swagger: `http://localhost:5130/swagger`

## 3. Usuarios

| Usuario         | Clave             | Rol    |
|-----------------|-------------------|--------|
| `admin`         | `Biblioteca2026!` | ADMIN  |
| `bibliotecario` | `biblio123`       | BIBLIO |
| `lector`        | `lector123`       | LECTOR |

ADMIN ve todo. BIBLIO gestiona catálogo y préstamos. LECTOR solo consulta el
catálogo y ve **Mis préstamos** con sus propias multas.

## 4. Orden para crear datos en la prueba

Usa **Nuevo registro → Guardar** en cada ventana. Primero crea lo que no
depende de nada:

1. Autores, Editoriales, Ubicaciones, Proveedores, Personas.
2. Idiomas y Categorías (piden Estado).
3. Libros (piden Editorial e Idioma).
4. Ejemplares (piden Estado, Libro y Ubicación).
5. Libros_Autores y Libros_Categorias.
6. Empleados, Préstamos y Préstamos_Libros.

Para prestar un libro, entra a **Prestamos**, busca la persona por nombre,
selecciónala, marca uno o varios ejemplares disponibles y registra. Al
devolver un préstamo atrasado se genera una multa de 2.000 pesos por día.

## 5. Verificación rápida

```bash
dotnet build biblioteca.sln
dotnet test biblioteca.sln --no-build
```

Resultado esperado: 20 pruebas superadas, 0 errores.
