using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PruebaTecnica.Infrastructure.Persistence.Migrations;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Extensions
{
    public static class DatabaseSeedingExtensions
    {
        /// <summary>
        /// Adds database seeding services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDatabaseSeeding(this IServiceCollection services)
        {
            // Register any services needed for seeding
            return services;
        }

        /// <summary>
        /// Seeds the database with initial data if tables are empty
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="logger">The logger</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider, ILogger logger)
        {
            await SeedData.SeedDatabaseAsync(serviceProvider, logger);
        }
    }
} 