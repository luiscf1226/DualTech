using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(
            IServiceProvider serviceProvider,
            ILogger<DatabaseInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                _logger.LogInformation("Starting database initialization...");

                // Apply migrations
                await ApplyMigrationsAsync(dbContext);

                // Seed data if needed
                await SeedDataAsync(dbContext);

                _logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private async Task ApplyMigrationsAsync(ApplicationDbContext dbContext)
        {
            _logger.LogInformation("Applying migrations...");
            
            // Check if database exists, if not create it
            await dbContext.Database.EnsureCreatedAsync();
            
            // Apply any pending migrations
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
                _logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                _logger.LogInformation("No migrations to apply.");
            }
        }

        private async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            _logger.LogInformation("Checking if data seeding is needed...");

            // Only seed if the database is empty
            if (!dbContext.Clientes.Any())
            {
                _logger.LogInformation("Seeding data...");

                // Seed Clientes
                var clientes = new List<Cliente>
                {
                    new Cliente { Nombre = "Juan Pérez", Identidad = "0801-1990-12345" },
                    new Cliente { Nombre = "María López", Identidad = "0501-1985-67890" },
                    new Cliente { Nombre = "Carlos Rodríguez", Identidad = "0101-1978-54321" }
                };
                await dbContext.Clientes.AddRangeAsync(clientes);

                // Seed Productos
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Laptop HP", Descripcion = "Laptop HP Pavilion 15.6\" Intel Core i5", Precio = 15000.00m, Existencia = 10 },
                    new Producto { Nombre = "Monitor Dell", Descripcion = "Monitor Dell 24\" Full HD", Precio = 3500.00m, Existencia = 15 },
                    new Producto { Nombre = "Teclado Logitech", Descripcion = "Teclado mecánico Logitech G Pro", Precio = 1200.00m, Existencia = 20 },
                    new Producto { Nombre = "Mouse Inalámbrico", Descripcion = "Mouse inalámbrico Logitech M185", Precio = 350.00m, Existencia = 30 }
                };
                await dbContext.Productos.AddRangeAsync(productos);

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Data seeded successfully.");
            }
            else
            {
                _logger.LogInformation("Data already exists. Skipping seeding.");
            }
        }
    }
} 