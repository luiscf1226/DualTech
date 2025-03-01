using System;
using System.Collections.Generic;

namespace PruebaTecnica.Domain.Entities
{
    public class Orden
    {
        public Orden()
        {
            DetallesOrdenes = new HashSet<DetalleOrden>();
            FechaCreacion = DateTime.Now;
        }

        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Navigation properties
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleOrden> DetallesOrdenes { get; set; }
    }
} 