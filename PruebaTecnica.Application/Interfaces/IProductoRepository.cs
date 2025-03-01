using PruebaTecnica.Domain.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<bool> UpdateExistencia(long id, long cantidad);
    }
} 