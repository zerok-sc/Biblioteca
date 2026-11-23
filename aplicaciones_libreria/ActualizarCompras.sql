USE biblioteca_db;
GO

IF OBJECT_ID('Compras_Libros') IS NULL
BEGIN
    CREATE TABLE [Compras_Libros] (
        [id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
        [compra] INT NOT NULL REFERENCES [Compras]([id]),
        [libro] INT NOT NULL REFERENCES [Libros]([id]),
        [cantidad] INT NOT NULL,
        [costo_unitario] DECIMAL(10, 2) NOT NULL
    );
END
GO
