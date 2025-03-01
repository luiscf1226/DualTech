using PruebaTecnica.Domain.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetClienteWithOrdenes(long id);
    }
} 