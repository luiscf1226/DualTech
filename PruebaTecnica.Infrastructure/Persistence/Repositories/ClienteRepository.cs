using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cliente> GetClienteWithOrdenes(long id)
        {
            return await _dbContext.Clientes
                .Include(c => c.Ordenes)
                .FirstOrDefaultAsync(c => c.ClienteId == id);
        }
    }
} 