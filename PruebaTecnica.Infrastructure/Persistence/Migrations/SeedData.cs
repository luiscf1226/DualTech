using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Migrations
{
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                logger.LogInformation("Starting database seeding...");

                // Apply migrations if any pending
                if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await dbContext.Database.MigrateAsync();
                }

                // Seed data only if tables are empty
                await SeedEntitiesAsync(dbContext, logger);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedEntitiesAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            // Only seed if the database is empty
            if (!await dbContext.Clientes.AnyAsync())
            {
                logger.LogInformation("Seeding Cliente data...");
                
                // Seed Clientes
                var clientes = new List<Cliente>
                {
                    new Cliente { Nombre = "Juan Pérez", Identidad = "0801-1990-12345" },
                    new Cliente { Nombre = "María López", Identidad = "0501-1985-67890" },
                    new Cliente { Nombre = "Carlos Rodríguez", Identidad = "0101-1978-54321" },
                    new Cliente { Nombre = "Ana Martínez", Identidad = "0401-1992-98765" },
                    new Cliente { Nombre = "Roberto Sánchez", Identidad = "0601-1980-13579" },
                    new Cliente { Nombre = "Laura Mendoza", Identidad = "0301-1995-24680" },
                    new Cliente { Nombre = "Fernando Gómez", Identidad = "0701-1988-97531" },
                    new Cliente { Nombre = "Patricia Flores", Identidad = "0201-1983-86420" },
                    new Cliente { Nombre = "Miguel Torres", Identidad = "0901-1975-75319" },
                    new Cliente { Nombre = "Sofía Ramírez", Identidad = "0501-1990-86421" }
                };
                
                await dbContext.Clientes.AddRangeAsync(clientes);
                await dbContext.SaveChangesAsync();
                
                logger.LogInformation($"Added {clientes.Count} clients to the database.");
            }

            if (!await dbContext.Productos.AnyAsync())
            {
                logger.LogInformation("Seeding Producto data...");
                
                // Seed Productos
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Laptop HP", Descripcion = "Laptop HP Pavilion 15.6\" Intel Core i5", Precio = 15000.00m, Existencia = 10 },
                    new Producto { Nombre = "Monitor Dell", Descripcion = "Monitor Dell 24\" Full HD", Precio = 3500.00m, Existencia = 15 },
                    new Producto { Nombre = "Teclado Logitech", Descripcion = "Teclado mecánico Logitech G Pro", Precio = 1200.00m, Existencia = 20 },
                    new Producto { Nombre = "Mouse Inalámbrico", Descripcion = "Mouse inalámbrico Logitech M185", Precio = 350.00m, Existencia = 30 },
                    new Producto { Nombre = "Impresora Epson", Descripcion = "Impresora multifuncional Epson EcoTank", Precio = 4500.00m, Existencia = 8 },
                    new Producto { Nombre = "Disco Duro Externo", Descripcion = "Disco duro externo Seagate 1TB", Precio = 1800.00m, Existencia = 12 },
                    new Producto { Nombre = "Memoria RAM", Descripcion = "Memoria RAM Kingston 8GB DDR4", Precio = 950.00m, Existencia = 25 },
                    new Producto { Nombre = "Tarjeta Gráfica", Descripcion = "Tarjeta gráfica NVIDIA GeForce RTX 3060", Precio = 8000.00m, Existencia = 5 },
                    new Producto { Nombre = "Auriculares Bluetooth", Descripcion = "Auriculares Bluetooth Sony WH-1000XM4", Precio = 3200.00m, Existencia = 10 },
                    new Producto { Nombre = "Webcam HD", Descripcion = "Webcam Logitech C920 HD Pro", Precio = 1500.00m, Existencia = 15 }
                };
                
                await dbContext.Productos.AddRangeAsync(productos);
                await dbContext.SaveChangesAsync();
                
                logger.LogInformation($"Added {productos.Count} products to the database.");
            }

            // Modified to ensure every client has at least one order
            if (!await dbContext.Ordenes.AnyAsync())
            {
                logger.LogInformation("Seeding Orden and DetalleOrden data...");
                
                // Get clients and products for reference
                var clientes = await dbContext.Clientes.ToListAsync();
                var productos = await dbContext.Productos.ToListAsync();
                
                // Create orders with details
                var ordenes = new List<Orden>();
                var random = new Random();
                
                // Create at least one order for each client
                foreach (var cliente in clientes)
                {
                    // Create 1-3 orders for each client
                    int numOrders = random.Next(1, 4);
                    
                    for (int i = 0; i < numOrders; i++)
                    {
                        // Create order
                        var orden = new Orden
                        {
                            ClienteId = cliente.ClienteId,
                            FechaCreacion = DateTime.Now.AddDays(-random.Next(1, 30)),
                            Impuesto = 0, // Will be calculated based on details
                            Subtotal = 0, // Will be calculated based on details
                            Total = 0,    // Will be calculated based on details
                            DetallesOrdenes = new List<DetalleOrden>()
                        };
                        
                        // Add 1-5 details to each order
                        var numDetails = random.Next(1, 6);
                        
                        // Create a set to track used product IDs to avoid duplicates in the same order
                        var usedProductIds = new HashSet<long>();
                        
                        for (int j = 0; j < numDetails; j++)
                        {
                            // Select a random product that hasn't been used in this order yet
                            Producto producto;
                            do
                            {
                                producto = productos[random.Next(productos.Count)];
                            } while (usedProductIds.Contains(producto.ProductoId) && usedProductIds.Count < productos.Count);
                            
                            // Add product ID to used set
                            usedProductIds.Add(producto.ProductoId);
                            
                            // Random quantity between 1 and 5
                            var cantidad = random.Next(1, 6);
                            
                            // Calculate values
                            var subtotal = producto.Precio * cantidad;
                            var impuesto = subtotal * 0.15m; // 15% tax
                            var total = subtotal + impuesto;
                            
                            // Create order detail
                            var detalle = new DetalleOrden
                            {
                                ProductoId = producto.ProductoId,
                                Cantidad = cantidad,
                                Subtotal = subtotal,
                                Impuesto = impuesto,
                                Total = total
                            };
                            
                            // Add detail to order
                            orden.DetallesOrdenes.Add(detalle);
                            
                            // Update order totals
                            orden.Subtotal += subtotal;
                            orden.Impuesto += impuesto;
                            orden.Total += total;
                        }
                        
                        ordenes.Add(orden);
                    }
                }
                
                await dbContext.Ordenes.AddRangeAsync(ordenes);
                await dbContext.SaveChangesAsync();
                
                logger.LogInformation($"Added {ordenes.Count} orders with details to the database.");
                logger.LogInformation($"Every client now has at least one order with product details.");
            }
        }
    }
} 