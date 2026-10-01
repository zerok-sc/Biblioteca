/* ============================================================
   PROYECTO: BIBLIOTECA
   Materia: Programación de Software - ITM
   Integrantes: Santiago Carmona Zapata
                Daniel Moncada Rodríguez
                Juan José Gómez Jaramillo */

USE master;
GO
IF DB_ID('biblioteca_db') IS NOT NULL
BEGIN
    ALTER DATABASE biblioteca_db SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE biblioteca_db;
END
GO
CREATE DATABASE biblioteca_db;
GO
USE biblioteca_db;
GO

CREATE TABLE [Personas] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [cedula] NVARCHAR(50) NOT NULL UNIQUE,
    [nombre] NVARCHAR(100) NOT NULL,
    [fecha_nacimiento] SMALLDATETIME NULL,
    [telefono] NVARCHAR(50) NULL
);

CREATE TABLE [Estados] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(50) NOT NULL,
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [descripcion] NVARCHAR(200) NULL,
    [fecha_creacion] SMALLDATETIME NOT NULL
);

CREATE TABLE [Editoriales] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(100) NOT NULL,
    [pais] NVARCHAR(50) NULL,
    [telefono] NVARCHAR(50) NULL,
    [correo] NVARCHAR(100) NULL
);

CREATE TABLE [Autores] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(100) NOT NULL,
    [apellido] NVARCHAR(100) NULL,
    [nacionalidad] NVARCHAR(50) NULL,
    [fecha_nacimiento] DATE NULL
);

CREATE TABLE [Proveedores] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(100) NOT NULL,
    [nit] NVARCHAR(50) NOT NULL UNIQUE,
    [telefono] NVARCHAR(50) NULL,
    [correo] NVARCHAR(100) NULL
);

CREATE TABLE [Empleados] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [persona] INT NOT NULL REFERENCES [Personas]([id]),
    [cargo] NVARCHAR(50) NOT NULL,
    [fecha_contratacion] SMALLDATETIME NOT NULL
);

CREATE TABLE [Salarios] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [empleado] INT NOT NULL REFERENCES [Empleados]([id]),
    [valor] DECIMAL(10, 2) NOT NULL,
    [fecha_inicio] SMALLDATETIME NULL,
    [fecha_fin] SMALLDATETIME NULL,
    [observaciones] NVARCHAR(200) NULL
);

CREATE TABLE [Idiomas] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(50) NOT NULL,
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [descripcion] NVARCHAR(200) NULL,
    [estado] INT NOT NULL REFERENCES [Estados]([id])
);

CREATE TABLE [Libros] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [nombre] NVARCHAR(200) NOT NULL,
    [editorial] INT NOT NULL REFERENCES [Editoriales]([id]),
    [idioma] INT NOT NULL REFERENCES [Idiomas]([id])
);

CREATE TABLE [Libros_Autores] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [libro] INT NOT NULL REFERENCES [Libros]([id]),
    [autor] INT NOT NULL REFERENCES [Autores]([id]),
    [tipo_participacion] NVARCHAR(50) NULL,
    [fecha_registro] SMALLDATETIME NOT NULL
);

CREATE TABLE [Categorias] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(50) NOT NULL,
    [descripcion] NVARCHAR(200) NULL,
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [estado] INT NOT NULL REFERENCES [Estados]([id])
);

CREATE TABLE [Libros_Categorias] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [libro] INT NOT NULL REFERENCES [Libros]([id]),
    [categoria] INT NOT NULL REFERENCES [Categorias]([id]),
    [fecha_asignacion] SMALLDATETIME NOT NULL,
    [estado] INT NOT NULL REFERENCES [Estados]([id])
);

CREATE TABLE [Secciones] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [nombre] NVARCHAR(50) NOT NULL,
    [descripcion] NVARCHAR(200) NULL,
    [ubicacion] NVARCHAR(100) NULL,
    [estado] INT NOT NULL REFERENCES [Estados]([id])
);

CREATE TABLE [Estanterias] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [numero] INT NOT NULL,
    [capacidad] INT NOT NULL,
    [seccion] INT NOT NULL REFERENCES [Secciones]([id])
);

CREATE TABLE [Ubicaciones] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [piso] INT NOT NULL,
    [descripcion] NVARCHAR(200) NULL,
    [estanteria] INT NOT NULL REFERENCES [Estanterias]([id])
);

CREATE TABLE [Ejemplares] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [estado] INT NOT NULL REFERENCES [Estados]([id]),
    [libro] INT NOT NULL REFERENCES [Libros]([id]),
    [ubicacion] INT NOT NULL REFERENCES [Ubicaciones]([id])
);

CREATE TABLE [Prestamos] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [codigo] NVARCHAR(50) NOT NULL UNIQUE,
    [persona] INT NOT NULL REFERENCES [Personas]([id]),
    [empleado] INT NOT NULL REFERENCES [Empleados]([id]),
    [fecha] SMALLDATETIME NOT NULL
);

CREATE TABLE [Prestamos_Libros] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [prestamo] INT NOT NULL REFERENCES [Prestamos]([id]),
    [ejemplar] INT NOT NULL REFERENCES [Ejemplares]([id]),
    [estado] INT NOT NULL REFERENCES [Estados]([id]),
    [fecha] SMALLDATETIME NOT NULL
);

CREATE TABLE [Multas] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [valor] DECIMAL(10, 2) NOT NULL,
    [fecha] SMALLDATETIME NOT NULL,
    [prestamo_libro] INT NOT NULL REFERENCES [Prestamos_Libros]([id]),
    [estado] INT NOT NULL REFERENCES [Estados]([id])
);

CREATE TABLE [Compras] (
    [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [fecha] SMALLDATETIME NOT NULL,
    [total] DECIMAL(10, 2) NOT NULL,
    [proveedor] INT NOT NULL REFERENCES [Proveedores]([id]),
    [empleado] INT NOT NULL REFERENCES [Empleados]([id])
);
GO

INSERT INTO [Personas] ([cedula], [nombre], [fecha_nacimiento], [telefono]) VALUES
('10101', 'Carlos Pérez',    '1990-03-15', '3001234567'),
('20202', 'María Gómez',     '1988-07-22', '3109876543'),
('30303', 'Juan Rodríguez',   '1995-11-05', '3201112233'),
('40404', 'Ana Martínez',     '1992-01-30', '3154445566'),
('50505', 'Luis López',       '1998-09-12', '3187778899');

INSERT INTO [Estados] ([nombre], [codigo], [descripcion], [fecha_creacion]) VALUES
('Activo',        'ACT', 'Registro activo dentro del sistema',        GETDATE()),
('Inactivo',      'INA', 'Registro deshabilitado',                    GETDATE()),
('Prestado',      'PRE', 'El ejemplar se encuentra prestado',         GETDATE()),
('Disponible',    'DIS', 'El ejemplar está disponible para préstamo', GETDATE()),
('Mantenimiento', 'MAN', 'El ejemplar está en reparación',            GETDATE());

INSERT INTO [Editoriales] ([nombre], [pais], [telefono], [correo]) VALUES
('Penguin Books',    'EE.UU.',   '6041234567', 'contacto@penguin.com'),
('Planeta',          'España',   '3412345678', 'contacto@planeta.es'),
('Alfaguara',        'Colombia', '6012345678', 'contacto@alfaguara.co'),
('Fondo de Cultura', 'México',   '5512345678', 'contacto@fce.mx'),
('Anagrama',         'España',   '3498765432', 'contacto@anagrama.es');

INSERT INTO [Autores] ([nombre], [apellido], [nacionalidad], [fecha_nacimiento]) VALUES
('Gabriel',  'García Márquez',    'Colombiana', '1927-03-06'),
('Miguel',   'de Cervantes',      'Española',   '1547-09-29'),
('George',   'Orwell',            'Británica',  '1903-06-25'),
('Antoine',  'de Saint-Exupéry',  'Francesa',   '1900-06-29'),
('Julio',    'Cortázar',          'Argentina',  '1914-08-26');

INSERT INTO [Proveedores] ([nombre], [nit], [telefono], [correo]) VALUES
('Distribuidora Panamericana', '800123456-1', '6041111111', 'ventas@panamericana.com'),
('Librería Nacional',          '800654321-2', '6042222222', 'ventas@nacional.com'),
('Impresos Populares',         '900112233-3', '6043333333', 'ventas@impresos.com'),
('Suministros Lectura',        '900445566-4', '6044444444', 'ventas@lectura.com'),
('Global Books',               '900778899-5', '6045555555', 'ventas@globalbooks.com');

INSERT INTO [Empleados] ([persona], [cargo], [fecha_contratacion]) VALUES
(1, 'Bibliotecario', '2020-01-15'),
(2, 'Archivista',    '2019-05-10'),
(3, 'Administrador', '2018-03-01'),
(4, 'Gerente',       '2017-08-20'),
(5, 'Asistente',     '2022-02-14');

INSERT INTO [Salarios] ([empleado], [valor], [fecha_inicio], [fecha_fin], [observaciones]) VALUES
(1, 1300000.00, '2024-01-01', NULL, 'Sueldo base'),
(2, 1800000.00, '2024-01-01', NULL, 'Especialista'),
(3, 2500000.00, '2024-01-01', NULL, 'Coordinador'),
(4, 3000000.00, '2024-01-01', NULL, 'Director'),
(5, 1500000.00, '2024-01-01', NULL, 'Auxiliar');

INSERT INTO [Idiomas] ([nombre], [codigo], [descripcion], [estado]) VALUES
('Español', 'ES', 'Idioma español',  1),
('Inglés',  'EN', 'Idioma inglés',   1),
('Francés', 'FR', 'Idioma francés',  1),
('Alemán',  'DE', 'Idioma alemán',   1),
('Italiano','IT', 'Idioma italiano', 1);

INSERT INTO [Libros] ([codigo], [nombre], [editorial], [idioma]) VALUES
('LIB001', 'Cien Años de Soledad', 3, 1),
('LIB002', 'Don Quijote',          2, 1),
('LIB003', '1984',                 1, 2),
('LIB004', 'El Principito',        5, 3),
('LIB005', 'Rayuela',              4, 1);

INSERT INTO [Libros_Autores] ([libro], [autor], [tipo_participacion], [fecha_registro]) VALUES
(1, 1, 'Autor Principal', GETDATE()),
(2, 2, 'Autor Principal', GETDATE()),
(3, 3, 'Autor Principal', GETDATE()),
(4, 4, 'Autor Principal', GETDATE()),
(5, 5, 'Autor Principal', GETDATE());

INSERT INTO [Categorias] ([nombre], [descripcion], [codigo], [estado]) VALUES
('Novela',          'Obras narrativas de ficción',   'CAT01', 1),
('Clásicos',        'Obras clásicas universales',    'CAT02', 1),
('Ciencia Ficción', 'Narrativa de anticipación',     'CAT03', 1),
('Infantil',        'Literatura para niños',         'CAT04', 1),
('Ensayo',          'Textos de reflexión y análisis','CAT05', 1);

INSERT INTO [Libros_Categorias] ([libro], [categoria], [fecha_asignacion], [estado]) VALUES
(1, 1, GETDATE(), 1),
(2, 2, GETDATE(), 1),
(3, 3, GETDATE(), 1),
(4, 4, GETDATE(), 1),
(5, 1, GETDATE(), 1);

INSERT INTO [Secciones] ([nombre], [descripcion], [ubicacion], [estado]) VALUES
('Sección Norte', 'Literatura general',        'Piso 1', 1),
('Sección Sur',   'Consulta y referencia',     'Piso 1', 1),
('Sección Este',  'Ciencia y tecnología',      'Piso 2', 1),
('Sección Oeste', 'Literatura infantil',       'Piso 2', 1),
('Reserva',       'Material de acceso restringido', 'Sótano', 1);

INSERT INTO [Estanterias] ([codigo], [numero], [capacidad], [seccion]) VALUES
('EST-A', 1, 100, 1),
('EST-B', 2, 100, 1),
('EST-C', 3,  80, 2),
('EST-D', 4,  50, 3),
('EST-E', 5, 120, 5);

INSERT INTO [Ubicaciones] ([codigo], [piso], [descripcion], [estanteria]) VALUES
('UBI-101', 1, 'Repisa 1 del estante A', 1),
('UBI-102', 1, 'Repisa 2 del estante A', 1),
('UBI-201', 1, 'Repisa 1 del estante B', 2),
('UBI-301', 2, 'Repisa 1 del estante D', 4),
('UBI-501', 0, 'Repisa baja del estante E', 5);

INSERT INTO [Ejemplares] ([codigo], [estado], [libro], [ubicacion]) VALUES
('EJM-001', 4, 1, 1),
('EJM-002', 4, 2, 2),
('EJM-003', 3, 3, 3),
('EJM-004', 4, 4, 4),
('EJM-005', 5, 5, 5);

INSERT INTO [Prestamos] ([codigo], [persona], [empleado], [fecha]) VALUES
('PRES-01', 1, 1, GETDATE()),
('PRES-02', 2, 2, GETDATE()),
('PRES-03', 3, 1, GETDATE()),
('PRES-04', 4, 3, GETDATE()),
('PRES-05', 5, 2, GETDATE());

INSERT INTO [Prestamos_Libros] ([prestamo], [ejemplar], [estado], [fecha]) VALUES
(1, 1, 3, DATEADD(DAY, 15, GETDATE())),
(2, 2, 3, DATEADD(DAY, 15, GETDATE())),
(3, 3, 3, DATEADD(DAY, 15, GETDATE())),
(4, 4, 3, DATEADD(DAY, 15, GETDATE())),
(5, 5, 3, DATEADD(DAY, 15, GETDATE()));

INSERT INTO [Multas] ([valor], [fecha], [prestamo_libro], [estado]) VALUES
( 5000.00, GETDATE(), 1, 1),
(12000.00, GETDATE(), 2, 1),
(    0.00, GETDATE(), 3, 2),
( 3000.00, GETDATE(), 4, 1),
(    0.00, GETDATE(), 5, 2);

INSERT INTO [Compras] ([fecha], [total], [proveedor], [empleado]) VALUES
(GETDATE(),  450000.00, 1, 3),
(GETDATE(), 1200000.00, 2, 3),
(GETDATE(),  300000.00, 3, 4),
(GETDATE(),  850000.00, 4, 4),
(GETDATE(), 2000000.00, 5, 3);
GO
