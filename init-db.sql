-- SQL Script for creating tables with proper relationships
-- Database: PruebaTecnicaDb

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PruebaTecnicaDb')
BEGIN
    CREATE DATABASE PruebaTecnicaDb;
END
GO

USE PruebaTecnicaDb;
GO

-- Enable IDENTITY_INSERT for all tables
SET IDENTITY_INSERT ON;
GO

-- Create Cliente table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cliente')
BEGIN
    CREATE TABLE Cliente (
        ClienteId INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        Identidad NVARCHAR(50) NOT NULL
    );
END
GO

-- Create Producto table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Producto')
BEGIN
    CREATE TABLE Producto (
        ProductoId INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        Precio DECIMAL(18, 2) NOT NULL,
        Existencia INT NOT NULL
    );
END
GO

-- Create Orden table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orden')
BEGIN
    CREATE TABLE Orden (
        OrdenId INT PRIMARY KEY IDENTITY(1,1),
        ClienteId INT NOT NULL,
        FechaCreacion DATETIME2 NOT NULL,
        Impuesto DECIMAL(18, 2) NOT NULL,
        Subtotal DECIMAL(18, 2) NOT NULL,
        Total DECIMAL(18, 2) NOT NULL,
        CONSTRAINT FK_Orden_Cliente FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
    );
END
GO

-- Create DetalleOrden table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetalleOrden')
BEGIN
    CREATE TABLE DetalleOrden (
        DetalleOrdenId INT PRIMARY KEY IDENTITY(1,1),
        OrdenId INT NOT NULL,
        ProductoId INT NOT NULL,
        Cantidad DECIMAL(18, 2) NOT NULL,
        Impuesto DECIMAL(18, 2) NOT NULL,
        Subtotal DECIMAL(18, 2) NOT NULL,
        Total DECIMAL(18, 2) NOT NULL,
        CONSTRAINT FK_DetalleOrden_Orden FOREIGN KEY (OrdenId) REFERENCES Orden(OrdenId),
        CONSTRAINT FK_DetalleOrden_Producto FOREIGN KEY (ProductoId) REFERENCES Producto(ProductoId)
    );
END
GO

-- Insert sample data if tables are empty
IF NOT EXISTS (SELECT TOP 1 * FROM Cliente)
BEGIN
    INSERT INTO Cliente (Nombre, Identidad) VALUES 
    ('Juan Pérez', '0801-1990-12345'),
    ('María López', '0501-1985-67890'),
    ('Carlos Rodríguez', '0101-1978-54321');
END
GO

IF NOT EXISTS (SELECT TOP 1 * FROM Producto)
BEGIN
    INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia) VALUES 
    ('Laptop HP', 'Laptop HP Pavilion 15.6" Intel Core i5', 15000.00, 10),
    ('Monitor Dell', 'Monitor Dell 24" Full HD', 3500.00, 15),
    ('Teclado Logitech', 'Teclado mecánico Logitech G Pro', 1200.00, 20),
    ('Mouse Inalámbrico', 'Mouse inalámbrico Logitech M185', 350.00, 30);
END
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX [IX_Orden_ClienteId] ON [dbo].[Orden]
(
    [ClienteId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_DetalleOrden_OrdenId] ON [dbo].[DetalleOrden]
(
    [OrdenId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_DetalleOrden_ProductoId] ON [dbo].[DetalleOrden]
(
    [ProductoId] ASC
)
GO

-- Disable IDENTITY_INSERT for all tables
SET IDENTITY_INSERT OFF;
GO

PRINT 'Database initialization completed successfully.'
GO 