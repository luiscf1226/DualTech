using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class OrdenRepository : Repository<Orden>, IOrdenRepository
    {
        public OrdenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Orden> GetOrdenWithDetalles(long id)
        {
            return await _dbContext.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.DetallesOrdenes)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.OrdenId == id);
        }

        public async Task<IEnumerable<Orden>> GetOrdenesByCliente(long clienteId)
        {
            return await _dbContext.Ordenes
                .Where(o => o.ClienteId == clienteId)
                .Include(o => o.DetallesOrdenes)
                .ToListAsync();
        }
    }
} 