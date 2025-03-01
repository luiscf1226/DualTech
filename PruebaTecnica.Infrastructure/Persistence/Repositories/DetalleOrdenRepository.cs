using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class DetalleOrdenRepository : Repository<DetalleOrden>, IDetalleOrdenRepository
    {
        public DetalleOrdenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<DetalleOrden>> GetDetallesByOrden(long ordenId)
        {
            return await _dbContext.DetallesOrdenes
                .Where(d => d.OrdenId == ordenId)
                .Include(d => d.Producto)
                .ToListAsync();
        }
    }
} 