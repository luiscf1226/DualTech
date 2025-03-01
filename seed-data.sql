-- Seed Data Script for PruebaTecnicaDb
-- This script inserts dummy data with proper foreign key relationships
-- All IDs are auto-incremented

USE PruebaTecnicaDb;
GO

-- Clear existing data (optional - comment out if you want to keep existing data)
DELETE FROM DetalleOrden;
DELETE FROM Orden;
DELETE FROM Producto;
DELETE FROM Cliente;
GO

-- Reset identity columns (optional - comment out if you want to continue from current values)
DBCC CHECKIDENT ('DetalleOrden', RESEED, 0);
DBCC CHECKIDENT ('Orden', RESEED, 0);
DBCC CHECKIDENT ('Producto', RESEED, 0);
DBCC CHECKIDENT ('Cliente', RESEED, 0);
GO

-- Insert Cliente data
INSERT INTO Cliente (Nombre, Identidad) VALUES 
('Juan Pérez', '0801-1990-12345'),
('María López', '0501-1985-67890'),
('Carlos Rodríguez', '0101-1978-54321'),
('Ana Martínez', '0401-1992-98765'),
('Roberto Sánchez', '0601-1980-13579'),
('Laura Mendoza', '0301-1995-24680'),
('Fernando Gómez', '0701-1988-97531'),
('Patricia Flores', '0201-1983-86420'),
('Miguel Torres', '0901-1975-75319'),
('Sofía Ramírez', '0501-1990-86421');
GO

-- Insert Producto data
INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia) VALUES 
('Laptop HP', 'Laptop HP Pavilion 15.6" Intel Core i5', 15000.00, 10),
('Monitor Dell', 'Monitor Dell 24" Full HD', 3500.00, 15),
('Teclado Logitech', 'Teclado mecánico Logitech G Pro', 1200.00, 20),
('Mouse Inalámbrico', 'Mouse inalámbrico Logitech M185', 350.00, 30),
('Impresora Epson', 'Impresora multifuncional Epson EcoTank', 4500.00, 8),
('Disco Duro Externo', 'Disco duro externo Seagate 1TB', 1800.00, 12),
('Memoria RAM', 'Memoria RAM Kingston 8GB DDR4', 950.00, 25),
('Tarjeta Gráfica', 'Tarjeta gráfica NVIDIA GeForce RTX 3060', 8000.00, 5),
('Auriculares Bluetooth', 'Auriculares Bluetooth Sony WH-1000XM4', 3200.00, 10),
('Webcam HD', 'Webcam Logitech C920 HD Pro', 1500.00, 15);
GO

-- Insert Orden data
-- Note: Using GETDATE() for FechaCreacion to get current date/time
INSERT INTO Orden (ClienteId, FechaCreacion, Impuesto, Subtotal, Total) VALUES 
(1, DATEADD(day, -10, GETDATE()), 2250.00, 15000.00, 17250.00),
(2, DATEADD(day, -9, GETDATE()), 525.00, 3500.00, 4025.00),
(3, DATEADD(day, -8, GETDATE()), 180.00, 1200.00, 1380.00),
(4, DATEADD(day, -7, GETDATE()), 52.50, 350.00, 402.50),
(5, DATEADD(day, -6, GETDATE()), 675.00, 4500.00, 5175.00),
(1, DATEADD(day, -5, GETDATE()), 270.00, 1800.00, 2070.00),
(2, DATEADD(day, -4, GETDATE()), 142.50, 950.00, 1092.50),
(3, DATEADD(day, -3, GETDATE()), 1200.00, 8000.00, 9200.00),
(4, DATEADD(day, -2, GETDATE()), 480.00, 3200.00, 3680.00),
(5, DATEADD(day, -1, GETDATE()), 225.00, 1500.00, 1725.00);
GO

-- Insert DetalleOrden data
-- Note: Each order has 1-3 details with different products
INSERT INTO DetalleOrden (OrdenId, ProductoId, Cantidad, Impuesto, Subtotal, Total) VALUES 
-- Order 1 details
(1, 1, 1, 2250.00, 15000.00, 17250.00),

-- Order 2 details
(2, 2, 1, 525.00, 3500.00, 4025.00),

-- Order 3 details
(3, 3, 1, 180.00, 1200.00, 1380.00),

-- Order 4 details
(4, 4, 1, 52.50, 350.00, 402.50),

-- Order 5 details
(5, 5, 1, 675.00, 4500.00, 5175.00),

-- Order 6 details
(6, 6, 1, 270.00, 1800.00, 2070.00),

-- Order 7 details
(7, 7, 1, 142.50, 950.00, 1092.50),

-- Order 8 details
(8, 8, 1, 1200.00, 8000.00, 9200.00),

-- Order 9 details
(9, 9, 1, 480.00, 3200.00, 3680.00),

-- Order 10 details
(10, 10, 1, 225.00, 1500.00, 1725.00),

-- Additional details for some orders
(1, 2, 2, 1050.00, 7000.00, 8050.00),
(1, 3, 3, 540.00, 3600.00, 4140.00),
(2, 4, 2, 105.00, 700.00, 805.00),
(3, 5, 1, 675.00, 4500.00, 5175.00),
(4, 6, 2, 540.00, 3600.00, 4140.00),
(5, 7, 3, 427.50, 2850.00, 3277.50);
GO

-- Verify data was inserted
PRINT 'Clientes insertados: ' + CAST((SELECT COUNT(*) FROM Cliente) AS VARCHAR);
PRINT 'Productos insertados: ' + CAST((SELECT COUNT(*) FROM Producto) AS VARCHAR);
PRINT 'Ordenes insertadas: ' + CAST((SELECT COUNT(*) FROM Orden) AS VARCHAR);
PRINT 'Detalles de orden insertados: ' + CAST((SELECT COUNT(*) FROM DetalleOrden) AS VARCHAR);
GO

-- Create a view to see orders with their details (optional)
IF OBJECT_ID('vw_OrdenesConDetalles', 'V') IS NOT NULL
    DROP VIEW vw_OrdenesConDetalles;
GO

CREATE VIEW vw_OrdenesConDetalles AS
SELECT 
    o.OrdenId,
    c.Nombre AS Cliente,
    o.FechaCreacion,
    o.Subtotal AS OrdenSubtotal,
    o.Impuesto AS OrdenImpuesto,
    o.Total AS OrdenTotal,
    p.Nombre AS Producto,
    d.Cantidad,
    d.Subtotal AS DetalleSubtotal,
    d.Impuesto AS DetalleImpuesto,
    d.Total AS DetalleTotal
FROM 
    Orden o
    INNER JOIN Cliente c ON o.ClienteId = c.ClienteId
    INNER JOIN DetalleOrden d ON o.OrdenId = d.OrdenId
    INNER JOIN Producto p ON d.ProductoId = p.ProductoId;
GO

PRINT 'Seed data script completed successfully.';
GO 