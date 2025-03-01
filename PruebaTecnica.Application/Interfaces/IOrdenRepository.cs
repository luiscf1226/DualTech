using PruebaTecnica.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Interfaces
{
    public interface IOrdenRepository : IRepository<Orden>
    {
        Task<Orden> GetOrdenWithDetalles(long id);
        Task<IEnumerable<Orden>> GetOrdenesByCliente(long clienteId);
    }
} 