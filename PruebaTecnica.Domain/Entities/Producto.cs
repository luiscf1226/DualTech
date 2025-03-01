using System;
using System.Collections.Generic;

namespace PruebaTecnica.Domain.Entities
{
    public class Producto
    {
        public Producto()
        {
            DetallesOrdenes = new HashSet<DetalleOrden>();
        }

        public long ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public long Existencia { get; set; }

        // Navigation properties
        public virtual ICollection<DetalleOrden> DetallesOrdenes { get; set; }
    }
} 