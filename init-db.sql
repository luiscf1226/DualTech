-- SQL Script for creating tables with proper relationships
-- Database: PruebaTecnicaDb

USE [PruebaTecnicaDb]
GO

-- Enable IDENTITY_INSERT for all tables
SET IDENTITY_INSERT ON;
GO

-- Create Cliente table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cliente](
        [ClienteId] [bigint] IDENTITY(1,1) NOT NULL,
        [Nombre] [nvarchar](100) NOT NULL,
        [Identidad] [nvarchar](50) NOT NULL,
        CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
        (
            [ClienteId] ASC
        )
    )
END
GO

-- Create Producto table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Producto](
        [ProductoId] [bigint] IDENTITY(1,1) NOT NULL,
        [Nombre] [nvarchar](100) NOT NULL,
        [Descripcion] [nvarchar](500) NULL,
        [Precio] [decimal](18, 2) NOT NULL,
        [Existencia] [bigint] NOT NULL,
        CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
        (
            [ProductoId] ASC
        )
    )
END
GO

-- Create Orden table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orden]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Orden](
        [OrdenId] [bigint] IDENTITY(1,1) NOT NULL,
        [ClienteId] [bigint] NOT NULL,
        [Impuesto] [decimal](18, 2) NOT NULL,
        [Subtotal] [decimal](18, 2) NOT NULL,
        [Total] [decimal](18, 2) NOT NULL,
        [FechaCreacion] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Orden] PRIMARY KEY CLUSTERED 
        (
            [OrdenId] ASC
        ),
        CONSTRAINT [FK_Orden_Cliente] FOREIGN KEY([ClienteId])
        REFERENCES [dbo].[Cliente] ([ClienteId])
        ON DELETE CASCADE
    )
END
GO

-- Create DetalleOrden table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleOrden]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DetalleOrden](
        [DetalleOrdenId] [bigint] IDENTITY(1,1) NOT NULL,
        [OrdenId] [bigint] NOT NULL,
        [ProductoId] [bigint] NOT NULL,
        [Cantidad] [decimal](18, 2) NOT NULL,
        [Impuesto] [decimal](18, 2) NOT NULL,
        [Subtotal] [decimal](18, 2) NOT NULL,
        [Total] [decimal](18, 2) NOT NULL,
        CONSTRAINT [PK_DetalleOrden] PRIMARY KEY CLUSTERED 
        (
            [DetalleOrdenId] ASC
        ),
        CONSTRAINT [FK_DetalleOrden_Orden] FOREIGN KEY([OrdenId])
        REFERENCES [dbo].[Orden] ([OrdenId])
        ON DELETE CASCADE,
        CONSTRAINT [FK_DetalleOrden_Producto] FOREIGN KEY([ProductoId])
        REFERENCES [dbo].[Producto] ([ProductoId])
    )
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

-- Insert sample data (optional)
-- Cliente sample data
INSERT INTO [dbo].[Cliente] ([Nombre], [Identidad])
VALUES 
    (N'Juan Pérez', N'0801-1990-12345'),
    (N'María López', N'0501-1985-67890'),
    (N'Carlos Rodríguez', N'0101-1978-54321');
GO

-- Producto sample data
INSERT INTO [dbo].[Producto] ([Nombre], [Descripcion], [Precio], [Existencia])
VALUES 
    (N'Laptop HP', N'Laptop HP Pavilion 15.6" Intel Core i5', 15000.00, 10),
    (N'Monitor Dell', N'Monitor Dell 24" Full HD', 3500.00, 15),
    (N'Teclado Logitech', N'Teclado mecánico Logitech G Pro', 1200.00, 20),
    (N'Mouse Inalámbrico', N'Mouse inalámbrico Logitech M185', 350.00, 30);
GO

-- Disable IDENTITY_INSERT for all tables
SET IDENTITY_INSERT OFF;
GO

PRINT 'Database initialization completed successfully.'
GO 