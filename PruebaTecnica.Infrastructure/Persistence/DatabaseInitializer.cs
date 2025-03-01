using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Infrastructure.Persistence.Migrations;
using System;
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
                await SeedDataAsync();

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

        private async Task SeedDataAsync()
        {
            _logger.LogInformation("Starting comprehensive data seeding...");
            
            // Use the enhanced seeding method from SeedData class
            await SeedData.SeedDatabaseAsync(_serviceProvider, _logger);
        }
    }
} 