using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseInitializer(this IServiceCollection services)
        {
            services.AddScoped<DatabaseInitializer>();
            return services;
        }

        public static async Task<IApplicationBuilder> InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseInitializer>>();

            try
            {
                logger.LogInformation("Initializing database...");
                await initializer.InitializeAsync();
                logger.LogInformation("Database initialization completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                // Continue with application startup even if database initialization fails
                // This allows the application to start and potentially recover when the database becomes available
            }

            return app;
        }
    }
} 