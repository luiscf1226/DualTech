using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Infrastructure.Persistence.Extensions;
using PruebaTecnica.Infrastructure.Persistence.Repositories;

namespace PruebaTecnica.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Register database initializer
            services.AddDatabaseInitializer();
            
            // Register database seeding
            services.AddDatabaseSeeding();

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IOrdenRepository, OrdenRepository>();
            services.AddScoped<IDetalleOrdenRepository, DetalleOrdenRepository>();

            return services;
        }
    }
} 