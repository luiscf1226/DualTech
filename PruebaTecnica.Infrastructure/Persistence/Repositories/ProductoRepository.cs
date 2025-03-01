using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> UpdateExistencia(long id, long cantidad)
        {
            var producto = await _dbContext.Productos.FindAsync(id);
            if (producto == null)
                return false;

            if (producto.Existencia < cantidad)
                return false;

            producto.Existencia -= cantidad;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
} 