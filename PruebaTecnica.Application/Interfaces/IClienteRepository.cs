using PruebaTecnica.Domain.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetClienteWithOrdenes(long id);
        
        /// <summary>
        /// Gets a cliente by their identidad (unique identifier)
        /// </summary>
        /// <param name="identidad">The identidad to search for</param>
        /// <returns>The cliente if found, null otherwise</returns>
        Task<Cliente> GetClienteByIdentidad(string identidad);
    }
} 