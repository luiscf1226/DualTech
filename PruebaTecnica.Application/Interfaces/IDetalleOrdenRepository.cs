using PruebaTecnica.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Interfaces
{
    public interface IDetalleOrdenRepository : IRepository<DetalleOrden>
    {
        Task<IEnumerable<DetalleOrden>> GetDetallesByOrden(long ordenId);
    }
} 